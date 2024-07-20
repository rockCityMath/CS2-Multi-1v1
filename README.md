<br />
<div align="center">
  <h3 align="center">CS2-Multi-1v1</h3>

  <p align="center">
    A spiritual successor to <a href="https://github.com/splewis/csgo-multi-1v1/tree/master">splewis' csgo-multi-1v1</a>
    <br />
    This plugin allows any number of players to be automatically placed into separate arenas to fight 1v1 in a ladder-style system where the winner moves up an arena rank, and the loser moves down. Player weapons are determined by the randomly generated "round type" and they will both use those same weapons for the entire round. Once the round starts, players will fight each other and when one kills the other, that player will receive a point and both players will respawn with full health and weapons, when the round ends, the player with the most points will win.
    <br />
    <br />
	<strong>Note: This plugin contains minimal features, has some bugs, and I'm not supporting it right now :( .</strong>
	</p>
</div>

<!-- GETTING STARTED -->
## Getting Started

### Notes
- While developing, I've been using a Google Cloud Platform Debian VM to host the server, I would strongly suggest using this or something similar.
- This plugin has not been tested on a windows server.

### Prerequisites
- A working CS2 server with CounterStrikeSharp installed.
	- https://docs.cssharp.dev/docs/guides/getting-started.html
- Any prerequisites required by CounterStrikeSharp or MetaMod: Source 2.
	- .NET runtime, etc...

### Installation
- Obtain a compiled version of the plugin either from source (recommended) or in the "Releases" section of the GitHub page (not recommended)
- Move the CS2Multi1v1/ directory from building the plugin to the /addons/counterstrikesharp/plugins/ directory in your CS2 server installation.
- Inside /counterstrikesharp/configs create the desired core.json and admin.json config files.
- Inside /game/csgo/cfg, place the provided server.cfg file.
	- Ensure that your server is using this cfg on startup and gameplay, I just deleted all the other .cfg files but you dont necessarily have to do this.

<!-- USAGE EXAMPLES -->
## Usage
- Currently only aim_redline_fp is supported out-of-the-box (see the maps section below).
- Start your CS2 server with the desired aim map.
	- I use +host_workshop_map 3070253400
- Don't allow more players on the server than arenas on the map.
- When starting your server, check the console to ensure that CS2Multi1v1 was loaded by CounterStrikeSharp.
- Join the server and a bot should be present, I would try to kill/be killed by the bot a few times to ensure the respawns/round handling works as expected. If not, see the admin commands below.
- As players join, they will be placed in the waiting queue then placed in an arena at the start of the next round.

## Maps
A major shortcoming of the current version is that aim_redline_fp is hardcoded in some of the automatic spawn/arena initialization logic. You should be able to use other maps but will need to use the admin commands below to manually initialize spawns and requeue players.

Automatically changing maps is currently not supported and you will need to use the admin commands below to setup the arenas if the map changes.

Generally, any map ported from a csgo-multi-1v1 map should work. Maps are expected to have 1 t and 1 ct spawn per arena and arenas need to be reasonably far from each other.

## Admin Commands
- !sq - Show players currently in the waiting queue.
- !rq - Empty all arenas and assign all players on the server to the waiting queue.
- !arenainfo - Console log a list of arenas with players and that arenas roundtype.
- !resetarenas - Remove all existing arenas, get current map spawns, then create new arenas.
	- Useful when using maps besides aim_redline_fp.
	- After arenas are reset, players will be to be requeued (!rq)
- !endround - Manually ends the round.

<!-- ROADMAP -->
## Roadmap

- [x] Player Matching
- [x] Arena Climbing
- [x] Random Round Types
- [ ] Round Type Preferences
- [ ] Weapon Preferences
- [ ] Multiple Map Support
- [ ] Persist Player Stats

<!-- CONTRIBUTING -->
## Contributing
Any contributions you make are **greatly appreciated**. Incomplete items from the roadmap are the most requested features and would be most helpful, along with bugfixes, but feel free to implement any other features you think would be fun.

If you would like your code to be reviewed and merged into this repo:

1. Fork the Project
2. Create your Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<strong>Please keep PRs limited in scope to the feature you are implementing or bug you are fixing (within reason).</strong>

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<!-- CONTACT -->
## Contact

Feel free to reach out with any questions or suggestions.

Discord - @easy.rs

<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

- splewis - Creator of the original mutli-1v1 plugin.
- roflmuffin - Creator of the CounterStrikeSharp library, I would not have made this in SourcePawn :)
- CounterStrikeSharp Community
