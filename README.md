# NewsBot
Bot sample using cognitive services (Bing News Search and LUIS)

## Pre-reqs - you'll need the following subscriptions

* Bing Search: https://www.microsoft.com/cognitive-services/en-US/subscriptions 
* LUIS: https://www.luis.ai

## LUIS Setup

* Head over to https://www.luis.ai/ApplicationList and navigate to New App - Import Existing Application
* Select the NewsBot.json file within this repo, you can add an optional name, then Import
* Train the model then Publish it
* Select App Settings and copy the model ID (App ID)
* Update the NewsDialog.cs with your LUIS model ID and LUIS subscription key

## Bing Search Setup
* Update the NewsDialog.cs with your Bing Search Api Key

## Bot Framework Setup
* Edit the web.config with your Bot Application Name, ID and Password

## F5

* Run the code and invoke on your favourite channel.
