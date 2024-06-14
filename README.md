# OpenRecall

**OpenRecall** Is my attempt to recreate a similar tool to Microsoft's Recall. It uses AI to monitor and log your activity.
Why would anyone use this? I don't know but please don't be evil and use it to monitor your employees or some other dystopian purpose.

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Features

- **Desktop Screenshots**: Periodically captures screenshots of your desktop.
- **Active Window Monitoring**: Keeps track of your active windows.
- **AI Description Generation**: Generates and stores detailed descriptions of your activities using AI.

## Installation

### Prerequisites

- .NET 8.0 SDK or later
- Supported OS (Currently only Windows)

### Steps

1. **Clone the Repository**
   ```bash
   git clone https://github.com/amir-halloul/OpenRecall.git
   ```
2. **Navigate to the Project Directory**
   ```bash
   cd OpenRecall
   ```
3. **Restore Dependencies**
   ```bash
   dotnet restore
   ```
4. **Build the Project**
   ```bash
   dotnet build
   ```
5. **Run the Application**
   ```bash
   dotnet run
   ```

## Usage

1. **Launch OpenRecall**: Open the application by running it from the command line or your preferred development environment.
2. **Leave CLI Open**: Leave the CLI open in the background while you work.
3. **View Logs**: View your logs in the SQLite DB in %localappdata%\openrecall.db

## Configuration

OpenRecall provides several configuration options to tailor its behavior to your needs:

- **OpenAI API Key**: The tool uses gpt-4o so you'll need an API key to use it. You can get one [here](https://openai.com/).
- **Snapshot Interval**: Set how frequently activity snapshots are taken.
- **Activity Snapshot Threshold**: How many snapshots are required to trigger an activity description.

Create a `config.json` file and place it at the same directory as the executable. An example configuration file is provided at `config.example.json`.

## Contributing

We welcome contributions to enhance OpenRecall! To contribute, follow these steps:

1. **Fork the Repository**
2. **Create a Feature Branch**
   ```bash
   git checkout -b feature/YourFeatureName
   ```
3. **Commit Your Changes**
   ```bash
   git commit -m 'Add some feature'
   ```
4. **Push to the Branch**
   ```bash
   git push origin feature/YourFeatureName
   ```
5. **Create a Pull Request**

## License

OpenRecall is licensed under the MIT License. You can view the full license [here](LICENSE).

## Contact

For questions, issues, or feature requests, please reach out to me at:

- **Email**: amirhalloul@gmail.com
- **GitHub Issues**: [https://github.com/amir-halloul/OpenRecall/issues](https://github.com/amir-halloul/OpenRecall/issues)

Thank you for your interest in OpenRecall!
