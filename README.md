# DS2 META
 Dark Souls 2 testing/debugging/practice tool  
 Based on a CE table by [Lord Radai](https://github.com/LordRadai)  
 
 Inspired by [DS-Gadget 3.0](https://github.com/JKAnderson/DS-Gadget) and uses [Property Hook](https://github.com/JKAnderson/PropertyHook) by [TKGP](https://github.com/JKAnderson/).  
 
 META stands for:  
M  
E  
T  
A  
 
# WARNING  
 There has been absolutely NO testing on this tool and online use. For offline use only. Use online at your own risk.  
 Even actions not locked behind a setting are considered unsafe for online use.  
 YOU HAVE BEEN WARNED  

# Known Issues  
 * Item give with the in game message box can sometimes crash the game. Turn on "Give Items Silently" in the settings menu. No sure if silent item give bans or not. 

## Requirements 
* [.NET 4.7.2](https://www.microsoft.com/net/download/thank-you/net472)  

## Installing  
* Extract contents of zip archive to it's own folder. You may have to run as admin if DS2S META crashes  

## Troubleshooting  
If you are having problems getting it to launch and you tried installing the packages above, right click the exe, go to properties, hit the compatability tab and hit "Run compatability troubleshooter". If it works, apply the settings windows found.  

You can also try adding DS2S META as an exception to your antivirus  

## Thank You  
**[TKGP](https://github.com/JKAnderson/)** Author of [DS Gadget](https://github.com/JKAnderson/DS-Gadget) and [Property Hook](https://github.com/JKAnderson/PropertyHook)  
**[Lord Radai](https://github.com/LordRadai)** Author of the CE table used for this tool and good mentor  
**[pseudostripy](https://github.com/pseudostripy)** Tester and contributor   

# Change Log 
### Beta 0.1

* Cosmetic changes, and added bonfire control  

* Getting ready for 1.0 

* Checkbox to live update max quantity (Scan inventory constantly. CPU intensive).  

### Beta 0.0.3.2

* Unoofed storing position. (Fixed crash)  

### Beta 0.0.3.1

* Bonfire menu fixed so it displays actual ascetic level  

* Fixed wrong offset for Max Held, causing max item to be broken  

### Beta 0.0.3
**Please update all files in your Resource folder**  

* Bonfire warp. Not enabled during multiplayer or searching for multiplayer.  

* Restrict now works as expected (I think)  

* Some spelling errors fixed  

* Items tab Quantity and Upgrade no longer default to min/max val  

* Item name and category updates  

* New Item category for weapon reskins. Not Online Safe.  

* Search box text now hilights when searchbox selected  

* Item Box no longer crashes cause tool due to empty item  

* Misc vanity changes  

* Max available to spawn in is now more accurate when you use an item - Tied to max and restrict checkbox  

* Items now check usage params. Option to unlock spawning in undroppable items.  

### Beta 0.0.2  

* Fixed Give Souls func  

* Added reset/max level buttons  

* Blank search now stores a position

* Items refresh on reload

### Beta 0.0.1  

* Items max quantity now looks at how many items are in your inventory  

* Individual speed factors  

* Search all now toggles filter  

* Misc cosmetic changes to positioning  

### Beta 0.0.0.8  

* Revamped Bonfire tab code and look

* Added Up, Down, Collision and Speed hotkeys

### Beta 0.0.0.7  

* Fixed bonfire unlock script no longer breaks Fire Keepers Dwelling Bonfire (Have to rest at it to unlock it)

* Added Internals tab to display character equipment info and other

* Bonfires tab works now kinda. Will make it nice looking later.

### Beta 0.0.0.6  

* Baby Jump DLL Compatability  

### Beta 0.0.0.5  

* Item Quantity, Infusion and Max level read from params  

* Unlock All Bonfires  

* Edit hollow level   

* Fixed max HP bug  


### Beta 0.0.0.4   

* Mostly working item infusion/upgrade menu. Everything should be accurate except melee weapons categories. Report if any are wrong.  

* Split goods into consumables, ammo, upgrade materials and useable items.  

### Beta 0.0.0.3   

* Fixed Update Message meme  

### Beta 0.0.0.2  

* Added Online notification and text colors.  

* Implimented Stored Positions start. These positions will end up changing later, most likely, so they will break eventually.  

### Beta 0.0.0.1  
* Initial beta release. Player, Stats and Item tab  
