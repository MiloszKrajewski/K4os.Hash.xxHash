#r ".fake/FakeLib.dll"
#load "build.tools.fsx"

open Fake

let solutions = Proj.settings |> Config.keys "Build"
let packages = Proj.settings |> Config.keys "Pack"

let clean () = !! "**/bin/" ++ "**/obj/" |> DeleteDirs
let restore () = solutions |> Seq.iter Proj.restore
let build () = solutions |> Seq.iter Proj.build
let test () = Proj.xtestAll ()
let release () = packages |> Proj.packMany
let publish apiKey = packages |> Seq.iter (Proj.publishNugetOrg apiKey)


Target "Clean" (fun _ -> clean ())

Target "Restore" (fun _ -> 
    restore ()
    Proj.snkGen "K4os.snk"
)

Target "Build" (fun _ -> build ())

Target "Rebuild" ignore

Target "Release" (fun _ -> release ())

Target "Test" (fun _ -> test ())

Target "Release:Nuget" (fun _ -> Proj.settings |> Config.valueOrFail "nuget" "accessKey" |> publish)

"Restore" ==> "Build" ==> "Rebuild" ==> "Release" ==> "Release:Nuget"
"Clean" ?=> "Restore"
"Clean" ==> "Rebuild"
"Test" ==> "Release"
"Build" ?=> "Test"

RunTargetOrDefault "Build"