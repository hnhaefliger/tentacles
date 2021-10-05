# Tentacles

This game was in part inspired by Microsoft's [Tentacles: Enter the Mind](https://www.microsoft.com/en-us/p/tentacles-enter-the-mind/9wzdncrfjb4n?activetab=pivot:overviewtab) but with the twist of being a first person character crawling around in an infinite cave system generated procedurally. The terrain is generated in chunks using 3D Perlin noise and a marching cubes algorithm. The vertices and triangles are calculated on a separate thread as the player approaches them and then marked for optimization and rendering (must be done on the main thread in unity). I am very open to someone improving the visuals.

![A little demo](https://github.com/hnhaefliger/tentacles/blob/main/demo/tentacles.mp4)