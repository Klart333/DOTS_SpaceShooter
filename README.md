# SPACE SHOOTER

I made the space shooter and wanted to focus on the collision for Performance testing. My first approach was to do a simple circular square magnitude distance check. That turned out to work way better than I was expecting it to. I was struggling to find the performance impact of the Collision System. Eventually I clocked it at around 1.0ms at >100k Rocks, which is very fast (imo). At 100k unity is struggling way more rendering all the rocks than me running collision (I tried turning off shadows and other settings on the mesh renderer and turned on GPU Instancing on the material, which I'm sure helped but did not have a very noticable impact). Looking at the suggested AABB I was not really expecting it to go faster; for my circumstances, where all the colliders are circles, I was quite convinced it was hard to beat. 

Results from testing are the following:

Simple sqrMagnitude circle collision - Avg. ~1.0ms at 110k Rocks 
AABB Collision System - Avg. ~1.2ms at 110k Rocks

Which in my mind makes sense, sqrMagnitude is extremely fast on modern CPUs and I'm not too sure of the details but I feel like having four comparisons really hamstrings the AABB. 
Of course it's still crazy fast, as it's basically one line of code. I can imagine that the CPU has trouble preloading the code because of all the conditionals.

I tried to be wary of memory while programming both approaches, and I don't think there's any significant difference between them, they are both very lean (except for the native array that I should probably cache and do some cool stuff with instead of just creating one every frame, even just switching to a persistent allocator would probably be a great idea). 

I realize that I maybe unneccesarily sent in more data in the NativeArray in the AABB, sending in the whole LocalTransform to get the width and height of the projectile. I use the width and height, or more specifically the radius, in the sqrMagnitude solution also, there it is hard-coded. I doubt that made an statistically significant difference though.

## Also

Because I move the world and not the player, checking collision with the player can be very optimized. What I did was compare the absolute value of x and y to the radius of the rock, like a simplified AABB. 

### also also
I think I made the build to smol, but that way it's actually playable :P
