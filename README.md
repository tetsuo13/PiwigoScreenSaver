# Piwigo Screen Saver

[![Actions Status](https://github.com/tetsuo13/PiwigoScreenSaver/workflows/Continuous%20integration/badge.svg)](https://github.com/tetsuo13/PiwigoScreenSaver/actions)

This is a Windows screen saver which displays random photos from your [Piwigo](https://piwigo.org) gallery.

Uses the [Piwigo Web API](https://github.com/Piwigo/Piwigo/wiki/Piwigo-Web-API) web service available on your Piwigo gallery using the login credentials you provide to display a randomly chosen photo on the screen. The random photo is chosen by the Piwigo API although it doesn't appear to be very random. It will scale the photo up to fit the display and slowly move it around the screen until the next random photo is retrieved.

Please note that this screen saver will download a photo from your Piwigo gallery every 30 seconds. Not only that, but it'll download the largest version of the photo that will fit on your screen. If you're on a metered Internet connection or are typically offline, then this screen saver may not be for you.

## Installation

1. [Download the latest release](https://github.com/tetsuo13/PiwigoScreenSaver/releases).
2. Right-click and "Extract All" to unzip the downloaded file.
3. Move **PiwigoScreenSaver.scr** to **C:\Windows\System32**
4. Navigate to **C:\Windows\System32**, right-click on **PiwigoScreenSaver.scr**, and click on Install. This will open the **Screen Saver Settings** dialog where you can configure the screen saver.
   - To configure, you will need to supply the URL to your Piwigo gallery and a username and password to log in.

## Upgrade

Follow the first 3 installation steps above.

## Developer Notes

The login credentials are stored in the registry under the key **HKCU:\Software\PiwigoScreenSaver** in encrypted form.

Unexpected errors are logged to a file in **C:\Users\CurrentUser\AppData\Local\Temp\PiwigoScreenSaver.log** for reference. Create the environment variable `PiwigoScreenSaverDebug` to also log debug-level messages.
