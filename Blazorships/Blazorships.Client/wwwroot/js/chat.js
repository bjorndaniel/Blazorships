let transportType = signalR.TransportType.WebSockets;
let http = new signalR.HttpConnection(`http://${document.location.host}/chathub`, { transport: transportType });
let connection = new signalR.HubConnection(http);
connection.start();
let isInitialized = false;
let updateGameMethod;
Blazor.registerFunction('Blazorships.Client.JsInterop.InitChat', function () {
    if (isInitialized) {
        return;
    }
    isInitialized = true;
    console.log('Initializing chat');
    connection.on("ReceiveMessage", (user, message) => {
        const encodedMsg = user + " says " + message;
        const li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("messagesList").appendChild(li);
    });

    document.getElementById("sendButton").addEventListener("click", event => {
        const user = document.getElementById("playerId").value;
        const message = document.getElementById("messageInput").value;
        const gameId = document.getElementById('gameId').value;
        connection.invoke("SendMessage", gameId, user, message).catch(err => console.error);
        event.preventDefault();
    });

    document.getElementById("btnPlay").addEventListener("click", event => {
        const user = document.getElementById("userName").value;
        connection.invoke("InitGame", user).catch(err => console.error);
        event.preventDefault();
    });

    document.getElementById("btnFire").addEventListener("click", event => {
        const playerId = document.getElementById('playerId').value;
        const gameId = document.getElementById('gameId').value;
        const row = document.getElementById('hfSelectedRow').value;
        const column = document.getElementById('hfSelectedColumn').value;
        connection
            .invoke("Fire", gameId, playerId, row, column)
            .catch(err => console.error);
        event.preventDefault();
    });

});
connection
    .on('GameUpdated', (gameId) => {
        console.log(gameId);
        localStorage.setItem('CurrentGameId', JSON.stringify({'Id' : gameId}));
        var button = document.getElementById('btnUpdateGame');
        button.click();
    });

connection.start().catch(err => console.error);