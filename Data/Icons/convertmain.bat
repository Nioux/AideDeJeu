set INKSCAPE=..\..\Ignore\inkscape\inkscape.com
set ANDROID_PATH=..\..\AideDeJeu\AideDeJeu.Android\Resources\
set UWP_PATH=..\..\AideDeJeu\AideDeJeu.UWP\
set IOS_PATH=..\..\AideDeJeu\AideDeJeu.iOS\Media.xcassets\

rem goto ios

:android

%INKSCAPE% -z -e %ANDROID_PATH%drawable\%1.png -w 1024 -h 1024 %2.svg

:uwp

%INKSCAPE% -z -e %UWP_PATH%%1.png -w 1024 -h 1024 %2.svg

:ios

md %IOS_PATH%%1.imageset\
%INKSCAPE% -z -e %IOS_PATH%%1.imageset\%1.png -w 1024 -h 1024 %2.svg
