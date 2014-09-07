#### ChampionWinRate

*calculate win rates by champion for ally vs enemy in League of Legends*

This only considers ranked solo and duo queue.

* Summoner: Type in summoner name here.
* Region: Choose region here.
* Go: Calculates win rates for Summoner and Region.
* Status: Initially invisible. Provides updates during loading. After loading, shows total number of games.
* Personal Win Rate: Initially invisible. Personal win rate for Summoner.
* Data: Initially blank. Columns are
	* Champion: Champion name.
	* Ally Games: Number of games that Summoner played with Champion. Does not count games where Summoner played as Champion.
	* Ally Win %: Win Percentage when Summoner played with Champion. Does not count games where Summoner played as Champion.
	* Enemy Win %: Win Percentage when Summoner played against Champion.
	* Enemy Games: Number of games that Summoner played against Champion.
* Minimum Number of Games: Filter for Data. Champion rows will only be displayed in Data if both Ally Games and Enemy Games are greater than or equal to Minimum Number of Games. Set Minimum Number of Games to 0 for no filtering.

#### Setup
Add a resource file ApiKeys.txt which contains the API key.

#### Credit
ChampionWinRate uses

* LoL API by Riot Games
* Newtonsoft.Json.dll by James Newton-King

#### Disclaimer
ChampionWinRate isn't endorsed by Riot Games and doesn't reflect the views or opinions of Riot Games or anyone officially involved in producing or managing League of Legends. League of Legends and Riot Games are trademarks or registered trademarks of Riot Games, Inc. League of Legends © Riot Games, Inc.
