using System.ClientModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.Chat;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Extensions;
using TravelCompanion.Domain.Models;
using TravelCompanion.Domain.Services;
using TravelCompanion.SDK.Clients;
using Exception = System.Exception;

namespace TravelCompanion.MAUI.Views
{
    [QueryProperty(nameof(TripId), "TripId")]
    public partial class ChatPage : ContentPage
    {
        private ChatMessage _systemMessage;
        public ObservableCollection<Message> Messages { get; set; }
        private readonly ChatClient _chatClient;
        private readonly TripClient _tripClient;
        private readonly TripChatClient _tripChatClient;
        private readonly TripEventClient _tripEventClient;
        private TripDto _trip;
        private TripChatDto _tripChat;

        public ChatPage(ChatClient chatClient, TripClient tripClient, TripChatClient tripChatClient, TripEventClient tripEventClient)
        {
            InitializeComponent();

            _chatClient = chatClient;
            _tripClient = tripClient;
            _tripChatClient = tripChatClient;
            _tripEventClient = tripEventClient;
        }

        public async Task<TripEventDto> CreateTripEventAsync(TripEventDto tripEventDto)
        {
            return await _tripEventClient.CreateTripEventAsync(tripEventDto);
        }

        private readonly ChatTool _createTripEventTool = ChatTool.CreateFunctionTool(
            functionName: nameof(CreateTripEventAsync),
            functionDescription: "Create a trip event",
            functionParameters: BinaryData.FromString("""
                                                      {
                                                          "type": "object",
                                                          "properties": {
                                                              "eventName": {
                                                                  "type": "string",
                                                                  "description": "The name of the event"
                                                              },
                                                              "venueName": {
                                                                  "type": "string",
                                                                  "description": "The name of the venue"
                                                              },
                                                              "venueAddress": {
                                                                  "type": "string",
                                                                  "description": "The address of the venue"
                                                              },
                                                              "venueCity": {
                                                                  "type": "string",
                                                                  "description": "The city where the venue is located"
                                                              },
                                                              "venueState": {
                                                                  "type": "string",
                                                                  "description": "The state where the venue is located"
                                                              },
                                                              "venueCountry": {
                                                                  "type": "string",
                                                                  "description": "The country where the venue is located"
                                                              },
                                                              "venuePostalCode": {
                                                                  "type": "string",
                                                                  "description": "The postal code of the venue"
                                                              },
                                                              "startDateTime": {
                                                                  "type": "string",
                                                                  "format": "date-time",
                                                                  "description": "The start date and time of the event"
                                                              },
                                                              "endDateTime": {
                                                                  "type": "string",
                                                                  "format": "date-time",
                                                                  "description": "The end date and time of the event"
                                                              },
                                                              "eventNotes": {
                                                                  "type": "string",
                                                                  "description": "Additional notes about the event"
                                                              }
                                                          },
                                                          "required": [ "eventName", "startDateTime", "endDateTime" ]
                                                      }
                                                      """)
        );

        private int _tripId;

        public int TripId
        {
            get => _tripId;
            set
            {
                _tripId = value;
                LoadTripAsync();
            }
        }

        private async Task LoadTripAsync()
        {
            _trip = await _tripClient.GetTripByIdAsync(_tripId);
            Messages = new ObservableCollection<Message>();
            MessagesListView.ItemsSource = Messages;

            var tripChats = await _tripChatClient.GetAllTripChatsForTripAsync(_tripId);
            var tripChatInfo = tripChats.FirstOrDefault();

            _systemMessage = new SystemChatMessage("You are a Travel Companion that should assist the user while traveling and give them a good experience. " +
                                                   "Here is some information about the trip: " + _trip.ConvertToJson());

            bool startNewChat = true;

            if (tripChatInfo == null)
            {
                // If no chat exists, create a new one
                _tripChat = new TripChatDto
                {
                    TripId = _trip.TripId,
                    Description = "Chat for trip " + _trip.TripId,
                    Chat = string.Empty // Initialize with an empty chat
                };
                _tripChat = await _tripChatClient.CreateTripChatAsync(_tripChat);
            }
            else
            {
                _tripChat = await _tripChatClient.GetTripChatByIdAsync(tripChatInfo.TripChatId);

                if (!string.IsNullOrWhiteSpace(_tripChat.Chat))
                {
                    startNewChat = false;
                    LoadExistingChatMessages(_tripChat.Chat);
                }
            }

            if (startNewChat)
            {
                await AddFirstMessageAsync();
            }
        }

        private void LoadExistingChatMessages(string chatContent)
        {
            var messages = JsonConvert.DeserializeObject<List<Message>>(chatContent);

            foreach (var message in messages)
            {
                Messages.Add(message);
            }
        }

        //private async void OnSendButtonClicked(object sender, EventArgs e)
        //{
        //    var messageText = MessageEntry.Text;
        //    if (!string.IsNullOrWhiteSpace(messageText))
        //    {
        //        // Add user's message to the list
        //        Messages.Add(new Message { Text = messageText, BackgroundColor = "LightGray", Role = "user" });
        //        MessageEntry.Text = string.Empty;

        //        try
        //        {
        //            // Prepare the message for the API
        //            var messages = new List<ChatMessage>
        //            {
        //                _systemMessage
        //            };

        //            foreach (var message in Messages)
        //            {
        //                switch (message.Role)
        //                {
        //                    case "user":
        //                        messages.Add(new UserChatMessage(message.Text));
        //                        break;
        //                    case "assistant":
        //                        messages.Add(new AssistantChatMessage(message.Text));
        //                        break;
        //                }
        //            }

        //            AsyncCollectionResult<StreamingChatCompletionUpdate> streamingResult = _chatClient.CompleteChatStreamingAsync(messages);

        //            var newMessage = new Message { BackgroundColor = "LightBlue", Role = "assistant", Text = string.Empty };
        //            Messages.Add(newMessage);

        //            await foreach (StreamingChatCompletionUpdate chatUpdate in streamingResult)
        //            {
        //                foreach (var updatePart in chatUpdate.ContentUpdate)
        //                {
        //                    //Concatenate the new part of the response with the existing response
        //                    newMessage.Text += updatePart.Text;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle any exceptions that occur during the API call
        //            Messages.Add(new Message { Text = $"Error: {ex.Message}", BackgroundColor = "Red" });
        //        }
        //    }
        //}

        private async void OnSendButtonClicked(object sender, EventArgs e)
        {
            var messageText = MessageEntry.Text;
            if (!string.IsNullOrWhiteSpace(messageText))
            {
                // Add user's message to the list
                Messages.Add(new Message { Text = messageText, BackgroundColor = "LightGray", Role = "user" });
                MessageEntry.Text = string.Empty;

                try
                {
                    // Prepare the message for the API
                    var messages = new List<ChatMessage>
                    {
                        _systemMessage
                    };

                    foreach (var message in Messages)
                    {
                        switch (message.Role)
                        {
                            case "user":
                                messages.Add(new UserChatMessage(message.Text));
                                break;
                            case "assistant":
                                messages.Add(new AssistantChatMessage(message.Text));
                                break;
                        }
                    }

                    ChatCompletionOptions options = new()
                    {
                        Tools = { _createTripEventTool },
                    };

                    var newMessage = new Message
                        { BackgroundColor = "LightBlue", Role = "assistant", Text = string.Empty };
                    Messages.Add(newMessage);

                    bool requiresAction;

                    do
                    {
                        requiresAction = false;
                        Dictionary<int, string> indexToToolCallId = new Dictionary<int, string>();
                        Dictionary<int, string> indexToFunctionName = new Dictionary<int, string>();
                        Dictionary<int, StringBuilder> indexToFunctionArguments = new Dictionary<int, StringBuilder>();
                        StringBuilder contentBuilder = new();
                        AsyncCollectionResult<StreamingChatCompletionUpdate> chatUpdates
                            = _chatClient.CompleteChatStreamingAsync(messages, options);

                        await foreach (StreamingChatCompletionUpdate chatUpdate in chatUpdates)
                        {
                            // Accumulate the text content as new updates arrive.
                            foreach (ChatMessageContentPart contentPart in chatUpdate.ContentUpdate)
                            {
                                contentBuilder.Append(contentPart.Text);

                                // Update UI on the main thread
                                MainThread.BeginInvokeOnMainThread(() => { newMessage.Text += contentPart.Text; });
                            }

                            // Handle tool calls as new updates arrive.
                            foreach (StreamingChatToolCallUpdate toolCallUpdate in chatUpdate.ToolCallUpdates)
                            {
                                // Track the tool call ID, function name, and arguments
                                if (toolCallUpdate.Id is not null)
                                {
                                    indexToToolCallId[toolCallUpdate.Index] = toolCallUpdate.Id;
                                }

                                if (toolCallUpdate.FunctionName is not null)
                                {
                                    indexToFunctionName[toolCallUpdate.Index] = toolCallUpdate.FunctionName;
                                }

                                if (toolCallUpdate.FunctionArgumentsUpdate is not null)
                                {
                                    StringBuilder argumentsBuilder =
                                        indexToFunctionArguments.TryGetValue(toolCallUpdate.Index,
                                            out StringBuilder existingBuilder)
                                            ? existingBuilder
                                            : new StringBuilder();
                                    argumentsBuilder.Append(toolCallUpdate.FunctionArgumentsUpdate);
                                    indexToFunctionArguments[toolCallUpdate.Index] = argumentsBuilder;
                                }
                            }

                            // Handle tool call completion
                            switch (chatUpdate.FinishReason)
                            {
                                case ChatFinishReason.ToolCalls:
                                    List<ChatToolCall> toolCalls = new List<ChatToolCall>();
                                    foreach ((int index, string toolCallId) in indexToToolCallId)
                                    {
                                        ChatToolCall toolCall = ChatToolCall.CreateFunctionToolCall(
                                            toolCallId,
                                            indexToFunctionName[index],
                                            indexToFunctionArguments[index].ToString());

                                        toolCalls.Add(toolCall);
                                    }

                                    // Add the assistant message with tool calls to the conversation history
                                    string content = contentBuilder.Length > 0 ? contentBuilder.ToString() : null;
                                    messages.Add(new AssistantChatMessage(toolCalls, content));

                                    foreach (ChatToolCall toolCall in toolCalls)
                                    {
                                        if (toolCall.FunctionName == nameof(CreateTripEventAsync))
                                        {
                                            TripEventDto tripEventDto =
                                                JsonConvert.DeserializeObject<TripEventDto>(toolCall.FunctionArguments);
                                            tripEventDto.TripId = _tripId;

                                            // Execute the network call on a background thread
                                            var toolResult = await Task.Run(() =>
                                                _tripEventClient.CreateTripEventAsync(tripEventDto));

                                            // Update UI on the main thread
                                            MainThread.BeginInvokeOnMainThread(() =>
                                            {
                                                messages.Add(new ToolChatMessage(toolCall.Id,
                                                    toolResult.ConvertToJson()));
                                            });
                                        }
                                    }

                                    requiresAction = true;
                                    break;
                            }
                        }
                    } while (requiresAction);

                    // Log or handle the conversation history
                    foreach (ChatMessage requestMessage in messages)
                    {
                        // Handle the conversation history for debugging or persistence.
                    }
                }
                catch (Exception ex)
                {
                    Messages.Add(new Message { Text = $"Error: {ex.Message}", BackgroundColor = "Red" });
                }
            }
        }


        private async Task AddFirstMessageAsync()
        {
            bool didAddMessage = false;

            var newMessage = new Message { BackgroundColor = "LightBlue", Role = "assistant", Text = string.Empty };
            Messages.Add(newMessage);

            try
            {
                // Prepare the message for the API
                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage("Your job is to generate the first message of a chat conversation for a travel companion. It should say something like, \"Hi [traveller's first name if you have it; if not, just say hi]! I know all about your trip to [destination name]. You can use this chat window to ask me questions about things to do on your trip.\" Your response should only be the content of the chat message - you should not add any context or introduction at all - you must just return the chat message. Thank you!")
                };

                StringBuilder sb = new StringBuilder();
                sb.Append("Please generate me a first chat message. ");

                if (MauiProgram.CurrentUser != null)
                {
                    sb.Append($"Info about the user: {MauiProgram.CurrentUser.ConvertToJson()}");
                }

                sb.Append($"Info about the trip: {_trip.ConvertToJson()}");

                messages.Add(new UserChatMessage(sb.ToString()));

                AsyncCollectionResult<StreamingChatCompletionUpdate> streamingResult = _chatClient.CompleteChatStreamingAsync(messages);
                
                await foreach (StreamingChatCompletionUpdate chatUpdate in streamingResult)
                {
                    foreach (var updatePart in chatUpdate.ContentUpdate)
                    {
                        //Concatenate the new part of the response with the existing response
                        newMessage.Text += updatePart.Text;
                    }
                }

                if (!string.IsNullOrWhiteSpace(newMessage.Text))
                    didAddMessage = true;
            }
            catch (Exception ex)
            {
                didAddMessage = false;
            }

            if (!didAddMessage)
            {
                newMessage.Text = "Hi! I know all about your trip. You can use this chat window to ask me questions about things to do on your trip.";
            }
        }

        private async void OnTripDetailButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//TripDetailPage");
        }

        private async void OnClearButtonClicked(object sender, EventArgs e)
        {
            if (_tripChat != null)
            {
                await _tripChatClient.DeleteTripChatAsync(_tripChat.TripChatId);
            }

            await LoadTripAsync();
        }
    }

    public class Message : INotifyPropertyChanged
    {
        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        public string BackgroundColor { get; set; }

        public string Role { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
