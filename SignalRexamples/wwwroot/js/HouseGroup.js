let lbl_houseJoined = document.getElementById("lbl_houseJoined");


let btn_un_gryffindor = document.getElementById("btn_un_gryffindor");
let btn_un_slytherin = document.getElementById("btn_un_slytherin");
let btn_un_hufflepuff = document.getElementById("btn_un_hufflepuff");
let btn_un_ravenclaw = document.getElementById("btn_un_ravenclaw");

let btn_gryffindor = document.getElementById("btn_gryffindor");
let btn_slytherin = document.getElementById("btn_slytherin");
let btn_hufflepuff = document.getElementById("btn_hufflepuff");
let btn_ravenclaw = document.getElementById("btn_ravenclaw");

let trigger_gryffindor = document.getElementById("trigger_gryffindor");
let trigger_slytherin = document.getElementById("trigger_slytherin");
let trigger_hufflepuff = document.getElementById("trigger_hufflepuff");
let trigger_ravenclaw = document.getElementById("trigger_ravenclaw");

// connection details


var connHouses = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/HouseGroup").build(); // default websocket


//subscrip the house
btn_gryffindor.addEventListener("click", function (event) {
    connHouses.send("JoinHouse", "Gryffinder");
    event.preventDefault();
});
btn_slytherin.addEventListener("click", function (event) {
    connHouses.send("JoinHouse", "Slytherin");
    event.preventDefault();
});
btn_hufflepuff.addEventListener("click", function (event) {
    connHouses.send("JoinHouse", "Hufflepuff");
    event.preventDefault();
});
btn_ravenclaw.addEventListener("click", function (event) {
    connHouses.send("JoinHouse", "Ravenclaw");
    event.preventDefault();
});
//subscrip the house

//Unsubscrip the house
btn_un_gryffindor.addEventListener("click", function (event) {
    connHouses.send("LeaveHouse", "Gryffinder");
    event.preventDefault();
});
btn_un_slytherin.addEventListener("click", function (event) {
    connHouses.send("LeaveHouse", "Slytherin");
    event.preventDefault();
});
btn_un_hufflepuff.addEventListener("click", function (event) {
    connHouses.send("LeaveHouse", "Hufflepuff");
    event.preventDefault();
});
btn_un_ravenclaw.addEventListener("click", function (event) {
    connHouses.send("LeaveHouse", "Ravenclaw");
    event.preventDefault();
});
//Unsubscrip the house

//trigger notification the house
trigger_gryffindor.addEventListener("click", function (event) {
    connHouses.send("TrigerHouseNotify", "Gryffinder");
    event.preventDefault();
});
trigger_slytherin.addEventListener("click", function (event) {
    connHouses.send("TrigerHouseNotify", "Slytherin");
    event.preventDefault();
});
trigger_hufflepuff.addEventListener("click", function (event) {
    connHouses.send("TrigerHouseNotify", "Hufflepuff");
    event.preventDefault();
});
trigger_ravenclaw.addEventListener("click", function (event) {
    connHouses.send("TrigerHouseNotify", "Ravenclaw");
    event.preventDefault();
});
//trigger the house

connHouses.on("NotifyAddMember", (houseName) => {
    toastr.info("New Member Added in " + houseName);
});

connHouses.on("NotifyRemoveMember", (houseName) => {
    toastr.error("Member Removed from " + houseName);
});

connHouses.on("tiggerNotify", (houseName) => {
    toastr.success("New trigger Notification lanched for " + houseName);
});


//subscription status

connHouses.on("subscriptionStatus", (strGroupJoined, houseName, hasSubscribed) => {
    lbl_houseJoined.innerHTML = strGroupJoined;
    if (hasSubscribed) {
        switch (houseName) {
            case 'slytherin':
                btn_slytherin.style.display = "none";
                btn_un_slytherin.style.display = "";
                break;
            case 'gryffinder':
                btn_gryffindor.style.display = "none";
                btn_un_gryffindor.style.display = "";
                break;
            case 'hufflepuff':
                btn_hufflepuff.style.display = "none";
                btn_un_hufflepuff.style.display = "";
                break;
            case 'ravenclaw':
                btn_ravenclaw.style.display = "none";
                btn_un_ravenclaw.style.display = "";
                break;
            default:
                break;
        }
        toastr.success("You have subscribed Successfully." + houseName)

    } else {
        switch (houseName) {
            case 'slytherin':
                btn_slytherin.style.display = "";
                btn_un_slytherin.style.display = "none";
                break;
            case 'gryffinder':
                btn_gryffindor.style.display = "";
                btn_un_gryffindor.style.display = "none";
                break;
            case 'hufflepuff':
                btn_hufflepuff.style.display = "";
                btn_un_hufflepuff.style.display = "none";
                break;
            case 'ravenclaw':
                btn_ravenclaw.style.display = "";
                btn_un_ravenclaw.style.display = "none";
                break;
            default:
                break;
        }
        toastr.success("You have Unsubscribed Successfully." + houseName)
    }
})

function fulfilled() {
    console.info("connection to user hub successfully")

}

function reject() {
    console.info("Failed connection to user hub")
}

// start connections

connHouses.start().then(fulfilled, reject);