var cloakCounter = document.getElementById("cloakCounter");
var stoneCounter = document.getElementById("stoneCounter");
var wandCounter = document.getElementById("wandCounter");
//Create connection

var connVotingStatus = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/votingStatus").build(); // default websocket

connVotingStatus.on("updateDealthyHallowCount", (wand, stone, cloak) => {
  
    cloakCounter.innerHTML = cloak.toString();
    stoneCounter.innerHTML = stone.toString();
    wandCounter.innerHTML = wand.toString();
})

function fulfilled() {
    console.info("connection to voting hub successfully")
    connVotingStatus.invoke("GetRaceStatus").then( (value) => {
        cloakCounter.innerHTML = value.cloak.toString();
        stoneCounter.innerHTML = value.stone.toString();
        wandCounter.innerHTML = value.wand.toString();
    })
 
}

function reject() {
    console.info("Failed connection to voting hub")
}

// start connections

connVotingStatus.start().then(fulfilled, reject);