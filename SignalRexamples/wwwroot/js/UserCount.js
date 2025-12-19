
//Create connection

var connUserCount = new signalR.HubConnectionBuilder()
       .withAutomaticReconnect()
       .configureLogging(signalR.LogLevel.Information)  // log the traces
       .withUrl("/hubs/userCount").build(); // default websocket

//var connUserCount2 = new signalR.HubConnectionBuilder()
 //   .withUrl("/hubs/userCount", signalR.HttpTransportType.LongPolling).build();   // transport type LongPolling

//var connUserCount = new signalR.HubConnectionBuilder()
 //   .withUrl("/hubs/userCount", signalR.HttpTransportType.ServerSentEvents).build();   // transport type SSE

//connect to methods that hub invokes aka receive notification from hub
connUserCount.on("updateTotalViews", (value) => {
    var newCount = document.getElementById("totalViewCount");
    newCount.innerHTML = value.toString();
})
connUserCount.on("updateToUsers", (value) => {
    var newCount = document.getElementById("CurrentTotalViewCount");
    newCount.innerHTML = value.toString();
})


connUserCount.onclose((error) => {  // then connection is closed
    document.body.style.background = "red";
});

connUserCount.onreconnected((connectionId) => {
    document.body.style.background = "green";
});

connUserCount.onreconnecting((error) => {
    document.body.style.background = "orange";
});

// invoke hub methods aka send notification to hub

function newWindowLoadedOnClient() {
    connUserCount.send("NewWindowLoaded"); // it not return value from server
}

function newWindowLoadedOnClient1() {
    connUserCount.invoke("NewWindowLoaded1","AnbuvelParameter").then((value) => console.log(value)); // parameter pass to hub methods.
}
function fulfilled() {
    console.info("connection to user hub successfully")
    newWindowLoadedOnClient();
    newWindowLoadedOnClient1();
}

function reject() {
    console.info("Failed connection to user hub")
}

// start connections

connUserCount.start().then(fulfilled, reject);