# Redmine Telegram Bot
A Telegram bot interface for Redmine REST api. Also the user interface is abstracted away from the Telegram API so it can be used from alternative text based shells (e.g. command line and other chats bots).

## Current Features:
- **Username based security:** Bot is only allowed to obey commands from previously specified usernames.
- **Usernames bound to Redmine secrets:** The Telegram usernames are bound to Redmine secrets so every command you send will be executed using your Redmine user credentials.
- **No database dependency:** There is no database for storing user data. It would be overkill for a little app but still the abstractions can allow you to use one if you need one.
- **Stateful communication:** You will communicate the bot in a stateful context, so no need for long single line commands with arguments.
- **Projects listing:** List all projects or filter the projects based on certain characters.
- **Issue creation:** Create new issues on projects with title and description fields.
- **Issue assignment:** Assign users to newly created issues.
- **Context cancellation:** You can cancel any command before the command is fully prepared for execution.

## Motivation of the Software:
It might seem unnecessary to create a bot/command interpreter for Redmine but of course I had my personal motivations for making one. My motivations for this project are based purely based on learning purposes and personal use cases.

### General Motivations:
- **Practicality:** Sometimes an idea or bug pops up from nowhere so you may need to create an issue for it immediately. Seems like Redmine is mostly designed to be used from a PC, so the mobile experience is not the best. Still, this bot is mostly tech savy friendly, not for everyone.
- **Security:** Your project management system may be behind some extensive security measures so direct access may not always be possible. So you can use partial features with little compromises.

### Personal Motivations:
- **Warmup for newer .NET versions:** I was mostly working with legacy code (.NET 4.x) in my full-time work so I didn't have much experience with .NET Core. I wanted to have hands on experience on it.
- **Practicing abstractions:** I wanted to challenge myself to write highly abstracted, isolated, testable and modular code. For example, this is suppossed to be a Telegram interface but it's carefully abstracted so it can be used from command line (and other chat bots theoritcally).
