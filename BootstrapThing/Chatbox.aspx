<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chatbox.aspx.cs" Inherits="BootstrapThing.Chatbox" %>

<!DOCTYPE html>
<html>
<head>
    <title>Chatbox Page</title>
    <link rel="stylesheet" type="text/css" href="styles.css" />
    <style>
        /* Modal Styles */
        .modal {
            display: none;
            position: fixed;
            z-index: 1;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            overflow: auto;
            background-color: rgba(0, 0, 0, 0.5);
        }

        .modal-content {
            background-color: #fefefe;
            margin: 20% auto;
            padding: 20px;
            border: 1px solid #888;
            width: 80%;
            text-align: center;
        }

        .modal-close {
            position: absolute;
            top: 10px;
            right: 10px;
            cursor: pointer;
        }

        /* Chat Message Styles */
        .message {
            margin-bottom: 10px;
        }

        .sender {
            font-weight: bold;
        }

        .moderator {
            color: firebrick;
        }

        .normaluser{
            color: midnightblue;
        }

        .username-label {
            font-weight: bold;
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form runat="server">
        <div class="container">
            <h1>Chatbox - Static</h1>
            <div class="chat-container">
                <asp:Panel ID="pnlMessages" runat="server">
                    <!-- Messages will be dynamically generated here -->
                </asp:Panel>
            </div>
            <asp:Panel ID="pnlChat" runat="server" CssClass="chat-form">
                <asp:TextBox ID="txtMessage" runat="server" placeholder="Type your message..."></asp:TextBox>
                <asp:Button ID="btnSend" runat="server" Text="Send" OnClientClick="sendChatMessage(); return false;" CssClass="button buttonCool" />
                <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" CssClass="button buttonCool" />

            </asp:Panel>
        </div>

        <!-- Modal -->
        <div id="myModal" class="modal">
            <div class="modal-content">
                <span class="modal-close" onclick="closeModal()">&times;</span>
                <h2>Error</h2>
                <p>An error occurred while sending the chat message.</p>
            </div>
        </div>


        <script>
            function getChatMessages() {
                fetch('/api/getchatmessages.ashx')
                    .then(response => response.json())
                    .then(data => {
                        const pnlMessages = document.getElementById('pnlMessages');

                        // Clear existing messages
                        pnlMessages.innerHTML = '';

                        data.Messages.forEach(message => {
                            const messageDiv = document.createElement('div');
                            messageDiv.classList.add('message');

                            const senderSpan = document.createElement('span');
                            senderSpan.textContent = message.Username + ':';
                            senderSpan.classList.add('sender');
                            if (message.IsMod) {
                                senderSpan.classList.add('moderator');
                            } else {
                                senderSpan.classList.add('normaluser');
                            }

                            const contentSpan = document.createElement('span');
                            contentSpan.textContent = message.Content;

                            messageDiv.appendChild(senderSpan);
                            messageDiv.appendChild(contentSpan);

                            pnlMessages.appendChild(messageDiv);
                        });
                    })
                    .catch(error => {
                        console.log("Error: " + error);
                    });
            }

            function sendChatMessage() {
                const message = document.getElementById('txtMessage').value;

                const params = new URLSearchParams();
                params.append('message', message);

                fetch('/api/sendchat.ashx?' + params.toString(), {
                    method: 'POST'
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.error === 'none') {
                            document.getElementById('txtMessage').value = '';
                            getChatMessages();
                        } else {
                            // Show modal and play MP3 file on error
                            const modal = document.getElementById('myModal');
                            const audio = new Audio('error.wav');
                            modal.style.display = 'block';
                            audio.play();
                        }
                    })
                    .catch(error => {
                        console.log("Error: " + error);
                    });
            }

            function closeModal() {
                const modal = document.getElementById('myModal');
                modal.style.display = 'none';
            }

            setInterval(getChatMessages, 500);
        </script>
    </form>
</body>
</html>
