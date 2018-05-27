# UnitTests

I've had trouble getting Unity3d working with unit-tests across platform and still be debuggable.

So I made a new project in [UnitTests](UnitTests.csproj) that can be used stand-alone (if you manually add the relevant Unity3d references), or manually added to the genererated _Visual Studio_ Solution file that Unity3d creates itself.

Not an ideal fix. And eventually I'll want to use the 'live' unit-testing features from Unity3d as well.

For the moment however, this is the easiest work-around I've come up with after arguing with VS, Unity3d, and NuGet.

Drop a line if you have a better solution. Pun intended.

