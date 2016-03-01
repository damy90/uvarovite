# TrackTour Video Editing Application
Today everyone has a smartphone with a camera and GPS. Sometimes with integrated fitness trackers, accelerometers and other sensors. Dedicated sports trackers and video streaming  cameras are also very popular among extreme athletes. People tagg some of the information they record during a video recording, such as place and time but most of that information lives in different services, has a separate purpose and is uploaded in different places. The video goes in social media, the track goes to a site for uploading tracks and the fitness tracking data goes to a health monitoring app. Even with specialized equipment, such as connected cameras it is still hard to find the right tools to combine all the data they record with an amateur grade tool. The equipment and software with such integrated functionality is still rare and expensive. And even then - the users are locked in a single platform.

This project aims to create a free, user friendly and flexible tool for displaying information like speed, orientation, timing and map inside a video. The input data may originate from different incompatible devices and platforms, such as a map service, GPS tracker, phone video camera, etc.
## Demo
[Demo vedeo](https://drive.google.com/file/d/0B9-BhHxB4VqLNERDRnk2Smt0UGM/view?usp=sharing)
## User guide
### Getting started
You must choose a video file, a GPS track file in GPX format and provide a save location

![Getting started](https://raw.githubusercontent.com/damy90/uvarovite/master/Screenshots/getting-started.png)
### Configure widgets
Most sizes and positions are in percents which allows to easily maintain the same layout across different video resolutions. The widgets are drawn on top of the each frame, which contents were passed as a .NET Graphic.
#### Map
Adds a map of the smallest possible area to contain the GPS route to the video within a given image size. The size of this widget is determined by the size of the track widget. The map is pulled as a static image from the Google Maps API and it requires an active internet connection. The map can be made transparent through the opacity setting.
#### Track
Draws the route as well as the path traveled so far with a specified height, width calculated to fit all recorded coordinates, position, line width and color. Travelled path drawing can be disabled on its own. The sizes and positions of the map widget and the position and orientation marker are related to the size and position of the track widget.

![GPS track settings](https://raw.githubusercontent.com/damy90/uvarovite/master/Screenshots/gps-track-settings.png)
#### Overlay image
Draws a custom image (for instance logo) on top of the video at a given position. Supported file formats are PNG with transparency and JPG.

![Overlay Image widget](https://raw.githubusercontent.com/damy90/uvarovite/master/Screenshots/overlay-image.png)
#### Position marker
When enabled the position marker marks the current position on the track with an elongated triangle pointing in the direction of movement. Color and size are configurable. The size is in pixels.

![Position Marker settings](https://raw.githubusercontent.com/damy90/uvarovite/master/Screenshots/position-marker-settings.png)
#### Speed and distance
These are text based widgets that draw the speed in km/h and distance traveled to the current position in hours as text in the video. Settings include font, font size in pixels, color and position as a percentage of the video size.

![Speed and distance widget settings](https://raw.githubusercontent.com/damy90/uvarovite/master/Screenshots/speed-and-distance-widget-settings.png)