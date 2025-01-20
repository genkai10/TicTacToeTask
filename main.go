package main

import (
    "fmt"
    "net/http"
    "github.com/gorilla/websocket"
)

var upgrader = websocket.Upgrader{
    CheckOrigin: func(r *http.Request) bool {
        return true
    },
}

var waitingPlayer *websocket.Conn

func handleConnections(w http.ResponseWriter, r *http.Request) {
    conn, err := upgrader.Upgrade(w, r, nil)
    if err != nil {
        fmt.Println("Error upgrading connection:", err)
        return
    }
    defer conn.Close()

    fmt.Println("A user connected:", conn.RemoteAddr())

    if waitingPlayer == nil {
        // No player waiting, set this player as waiting
        waitingPlayer = conn
        conn.WriteMessage(websocket.TextMessage, []byte("Waiting for an opponent..."))
    } else {
        // Match found, notify both players
        waitingPlayer.WriteMessage(websocket.TextMessage, []byte("Match found! Starting game..."))
        conn.WriteMessage(websocket.TextMessage, []byte("Match found! Starting game..."))
        
        // Handle game logic here
        gameRoom := []*websocket.Conn{waitingPlayer, conn}
        waitingPlayer = nil
        handleGame(gameRoom)
    }
}

func handleGame(players []*websocket.Conn) {
    defer players[0].Close()
    defer players[1].Close()

    for {
        // Listen to player moves and forward them
        _, move, err := players[0].ReadMessage()
        if err != nil {
            fmt.Println("Error reading message:", err)
            break
        }
        players[1].WriteMessage(websocket.TextMessage, move)

        // Swap players
        players[0], players[1] = players[1], players[0]
    }
}

func main() {
    http.HandleFunc("/ws", handleConnections)

    port := "8080"
    fmt.Println("Server is listening on port", port)
    err := http.ListenAndServe(":"+port, nil)
    if err != nil {
        fmt.Println("Error starting server:", err)
    }
}