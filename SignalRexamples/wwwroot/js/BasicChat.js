var connBasicChat = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/basicChat").build(); // default websocket


document.getElementById("sendMessage").disabled = true;

connBasicChat.on("MessageRecieved", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user} - ${message}`;
});

document.getElementById("sendMessage").addEventListener("click", function (event) {
    var sender = document.getElementById("senderEmail").value;
    var reveicer = document.getElementById("receiverEmail").value;
    var message = document.getElementById("chatMessage").value;

    if (reveicer.length > 0) {
        connBasicChat.send("SendMessageToReceiver", sender, reveicer, message).catch(function (err) {
            console.error(err.toString());
        });
    } else {
        connBasicChat.send("SendMessageToAll", sender, message).catch(function (err) {
            console.error(err.toString());
        });
    }
    event.preventDefault();
    });





// start connections

connBasicChat.start().then(function () {
    document.getElementById("sendMessage").disabled = false;
});