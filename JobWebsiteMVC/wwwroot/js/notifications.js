"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    //.configureLogging(signalR.LogLevel.Information)
    .withAutomaticReconnect()
    .build();

connection.start().then(function () {
    //console.log("connection started!");
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("SendMessage", function (userid, message) {
    console.log(`${userid} sent "${message}"`);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
});



