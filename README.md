# Piwigo Screen Saver

[![Actions Status](https://github.com/tetsuo13/PiwigoScreenSaver/workflows/Continuous%20integration/badge.svg)](https://github.com/tetsuo13/PiwigoScreenSaver/actions)

This is a Windows screen saver which displays random photos from your Piwigo gallery.

Uses the [Piwigo Web API](https://piwigo.org/doc/doku.php?id=dev:webapi:start) available on your Piwigo gallery using the login credentials you provide to display a randomly chosen photo on the screen. The random photo is chosen by Piwigo. It will place the photo at a random location on the screen. Nothing fancy.

Please note that this screen saver will download a photo from your Piwigo gallery every 30 seconds. Not only that, but it'll download the largest version of the photo that will fit on your screen. If you're on a metered Internet connection or are typically offline, then this screen saver may not be for you.

## Installation

1. [Download the latest release](https://github.com/tetsuo13/PiwigoScreenSaver/releases).
2. Right-click and "Extract All" to unzip the downloaded file.
3. Move **PiwigoScreenSaver.scr** to **C:\Windows\System32**
4. Navigate to **C:\Windows\System32**, right-click on **PiwigoScreenSaver.scr**, and click on Install. This will open the **Screen Saver Settings** dialog where you can configure the screen saver.

## Developer Notes

The login credentials are stored in the registry under the key **HKCU:\Software\PiwigoScreenSaver** in encrypted form.

Unexpected errors are logged to a file in **C:\Users\CurrentUser\AppData\Local\Temp\PiwigoScreenSaver.log** for reference.
