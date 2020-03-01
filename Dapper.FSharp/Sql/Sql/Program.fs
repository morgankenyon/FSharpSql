// Learn more about F# at http://fsharp.org

open System
open FSharp.Control.Tasks.V2
open Microsoft.Data.SqlClient
open System.Data
open Dapper.FSharp
open Dapper.FSharp.MSSQL

type Person = {
    Id : Guid
    FirstName : string
    LastName : string
    Position : int
    DateOfBirth : DateTime option
}

let insertPersonDateTime (conn : IDbConnection) =
    let newPerson = { Id = Guid.NewGuid(); FirstName = "Roman"; LastName = "Provaznik"; Position = 1; DateOfBirth = None }
    
    insert {
        table "Persons"
        value newPerson
    } |> conn.InsertAsync

let insertPersonNoDateTime (conn : IDbConnection) =

    insert {
        table "Persons"
        value {| Id = Guid.NewGuid(); FirstName = "Without"; LastName = "Birth date"; Position = 3 |}
    } |> conn.InsertAsync

let RunDapperFSharp =
    task {
        use conn : IDbConnection = new SqlConnection("Data Source=LAPTOP-M5JK4R1J;Initial Catalog=DataSample;Integrated Security=True") :> IDbConnection
        conn.Open()
        let! numOfPersons = insertPersonDateTime conn
        let! numOfPersonsNoDateTime = insertPersonNoDateTime conn
        return numOfPersonsNoDateTime
    }

[<EntryPoint>]
let main argv =
    Dapper.FSharp.OptionTypes.register()

    RunDapperFSharp 
    |> Async.AwaitTask
    |> Async.RunSynchronously
    |> ignore
    printfn "Hello World from F#!"
    0
