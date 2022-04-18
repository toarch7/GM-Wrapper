
# GM-Wrapper

A reimplementation of MrPowerGamerBR's [Droidtale](https://github.com/MrPowerGamerBR/Droidtale/) GameMaker game android wrapper written in C#

# Usage

- You must have .NET Framework 4.6.1+ and Java Runtime Enviroment installed
- Find a fitting runtime for the game you're going to wrap.
- [Download](https://github.com/toarch7/GM-Wrapper/releases) and extract GM-Wrapper into game's directory

![Example 1](https://i.imgur.com/ltF6HWG.png)

- Run GM-Wrapper.exe 
- Wait until it prompts for exit

![Example 2](https://i.imgur.com/CsmxFth.png)

- The result package, base_signed.apk, can be installed, but before it's recommended to change package name.

# Things to do:

- Implement action menu
- Add APK Editing functionality (package name editor, custom keystore support, etc)
- Turn it into UndertaleModTool script
- Automatic runtime detection

# Known Issues:

- (Not wrapper-related) Nested directories aren't properly handled by GameMaker, so it might cause weird behaviour.

# Tools used
[iBotPeaches's APK Tool](https://github.com/iBotPeaches/Apktool)

[MrPowerGamerBR's Signer and Keystores](https://github.com/MrPowerGamerBR/Droidtale)
