
var ChatConn = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/groupChatHub")
    //.WithAutomaticReconnect([0,1000,3000,null])
    .build(); // default websocket


ChatConn.on("ReceivedUserConnected", function (userId, userName) {
        addMessage(`${userName} is Online`);
});
ChatConn.on("ReceivedUserDisConnected", function (userId, userName) {
        addMessage(`${userName} is Offline`);
})
ChatConn.on("ReceiveAddRoomMsg", function (maxRoom,roomId,roomName,userId, userName) {  
    addMessage(`${userName} has created romm ${roomName}`);
})
ChatConn.on("ReceiveRemoveRoomMsg", function (maxRoom,roomId,roomName,userId, userName) {
   addMessage(`${userName} has Deleted this ${roomName} room`);  
})
ChatConn.on("ReceivePublicMsg", function (roomId, userId, userName, message, roomName) {
    addMessage(`[Public Message :${roomName}] ${userName} says --> ${message}`);
})
ChatConn.on("ReceivePrivateMsg", function (senderId, sendername, receiverId, message, newId,receiverName) {
    addMessage(`[Private Message :${ReceiverName}] ${sendername} says --> ${message}`);
})
function addnewRoom(maxRoom) {

    let createRoomName = document.getElementById('createRoomName');

    var roomName = createRoomName.value;

    if (roomName == null && roomName == '') {
        return;
    }

    $.ajax({
        url: '/ChatRooms/PostChatRoom',
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ Id: 0, Name: roomName }),
        async: true,
        processData: false,
        cache: false,
        success: function (json) {
            createRoomName.value = '';
            ChatConn.send("SendMessageForChatRoom", maxRoom, json.id, json.name,1);
            fillRoomDropDown();
        },
        error: function (xhr) {
            alert('error');
        }
    })
}

function RemoveRoom(maxRoom) {
    let DelRoomName = document.getElementById('ddlDelRoom');
    var roomId = DelRoomName.value;
    var roomName = DelRoomName.options[DelRoomName.selectedIndex].text;
    if (roomId == null && roomId == '') {
        return;
    }
    $.ajax({
        url: '/ChatRooms/DelecteChatRoom/' +roomId,
        dataType: "json",
        type: "DELETE",
        success: function (json) {
            ChatConn.send("SendRemoveMessageForChatRoom", json.deleted, json.selected, roomName,2);
            fillRoomDropDown();
        },
        error: function (xhr) {
            alert(xhr);
        }
    });

}

document.addEventListener('DOMContentLoaded', (event) => {
    fillRoomDropDown();
    fillUserDropDown();
})

function sendPublicMessage() {
    let pMsg = document.getElementById("txtPublicMessage");


    let selRoom = document.getElementById('ddlSelRoom');
    var RoomId = selRoom.value;
    var RoomName = selRoom.options[selRoom.selectedIndex].text;

    ChatConn.send("sendPublicMessage", Number(RoomId), pMsg.value, RoomName);
}
function sendPrivateMessage() {
    let pMsg = document.getElementById("txtPrivateMessage");


    let receiver = document.getElementById('ddlSelUser');
    var receiverId = receiver.value;
    var receiverName = receiver.options[receiver.selectedIndex].text;

    ChatConn.send("sendPrivateMessage", receiverId, pMsg.value, receiverName);
 
}


function fillUserDropDown() {

    $.getJSON('/ChatRooms/GetChatUsers')
        .done(function (json) {

            var ddlSelUser = document.getElementById("ddlSelUser");

            ddlSelUser.innerText = null;

            json.forEach(function (item) {
                var newOption = document.createElement("option");

                newOption.text = item.userName;//item.whateverProperty
                newOption.value = item.id;
                ddlSelUser.add(newOption);


            });

        })
        .fail(function (jqxhr, textStatus, error) {

            var err = textStatus + ", " + error;
            console.log("Request Failed: " + jqxhr.detail);
        });

}

function fillRoomDropDown() {

    $.getJSON('/ChatRooms/GetChatRoom')
        .done(function (json) {
            var ddlDelRoom = document.getElementById("ddlDelRoom");
            var ddlSelRoom = document.getElementById("ddlSelRoom");

            ddlDelRoom.innerText = null;
            ddlSelRoom.innerText = null;

            json.forEach(function (item) {
                var newOption = document.createElement("option");

                newOption.text = item.name;
                newOption.value = item.id;
                ddlDelRoom.add(newOption);


                var newOption1 = document.createElement("option");

                newOption1.text = item.name;
                newOption1.value = item.id;
                ddlSelRoom.add(newOption1);

            });

        })
        .fail(function (jqxhr, textStatus, error) {

            var err = textStatus + ", " + error;
            console.log("Request Failed: " + jqxhr.detail);
        });

}

function addMessage(msg) {
    if (msg == null && msg == '') {
        return;
    }
    let ui = document.getElementById("messagesList");
    let li = document.createElement("li");
    li.innerHTML = msg;
    ui.appendChild(li);
}

ChatConn.start().then(function () {
   
});