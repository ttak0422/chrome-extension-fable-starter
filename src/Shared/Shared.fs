namespace Shared

open Thoth.Json

// TODO
// popupではカウントのインクリメントのみ
// optionではshareから拝借したcounterを使って初期値を改変
// 初期値はパーミッションを取得しストレージに保存
type Counter =
    { Count: int }

type Counter with
    static member Default = { Count = 0 }

module Counter =
    [<LiteralAttribute>]
    let private STORAGE_KEY = "Chrome"

    let save (counter: Counter): unit =
        Encode.Auto.toString (0, counter)
        |> fun json -> (STORAGE_KEY, json)
        |> Browser.WebStorage.localStorage.setItem

    let load(): Result<Counter, string> =
        Browser.WebStorage.localStorage.getItem STORAGE_KEY
        |> unbox
        |> Decode.fromString (Decode.Auto.generateDecoder<Counter>())

    let loadWithDefault(): Counter =
        load()
        |> function
        | Ok counter -> counter
        | _ -> Counter.Default
