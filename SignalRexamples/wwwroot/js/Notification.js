
let SendButton = document.getElementById("sendButton");
let NotificationInput = document.getElementById("notificationInput");
let NotificationCounter = document.getElementById("notificationCounter");
let NotificationBell = document.getElementById("notificationBell");
let MessageList = document.getElementById("messageList");
//let li = document.createElement("li");
var connNotifiy = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/notification").build(); // default websocket


SendButton.addEventListener("click", function (event) {
 
    LoadNotification();
    
    event.preventDefault();
});
connNotifiy.on("NotificationStatus", (MessageList) => {
    loadMessage(MessageList); 

});

function LoadNotification()
{
  
    let notifiyMessage = NotificationInput.value != null ? NotificationInput.value : null;
    connNotifiy.invoke("SendNotification", notifiyMessage).then((value) => {
        console.log(value);
        loadMessage(value); 
    })
    .catch(error => console.error(error));
     

}
function loadMessage(value) {
    if (value) {
        MessageList.innerHTML = "";
        NotificationInput.innerText = "";
        NotificationCounter.innerText = "(" + value.length.toString() + ")";
        value.length > 0 ? NotificationBell.style.color = "blue" : "";
        value.forEach(msg => {
            let li = document.createElement("li");
            li.innerHTML = `<a class="dropdown-item" href="#">${msg}</a>`;
            MessageList.appendChild(li);
        });
    } else {
        NotificationBell.style.color = "";
        MessageList.innerHTML = "";
    }
}



// start connections

connNotifiy.start().then(function (){
    console.info("connection to user hub successfully")
    LoadNotification();
});