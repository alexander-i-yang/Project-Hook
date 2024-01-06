using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace A2DK.Phys {
    public abstract class Solid : PhysObj
    {
        public override bool MoveGeneral(Vector2 direction, int magnitude, Func<PhysObj, Vector2, bool> onCollide) {
            if (magnitude < 0) throw new ArgumentException("Magnitude must be >0");

            int remainder = magnitude;

            Actor[] allActors = AllActors();
            
            // If the actor moves at least 1 pixel, Move one pixel at a time
            while (remainder > 0) {
                CheckCollisions(Vector2.zero, (p, d) => {
                    bool ret = p != this && onCollide(p, d);
                    if (ret) {
                        Debug.LogError("Stuck against" + p);
                    }

                    return ret;
                });

                HashSet<Actor> ridingActors = new HashSet<Actor>(allActors.Where(c => c.IsRiding(this)));
                bool collision = CheckCollisions<Actor>(direction, (a, d) => {
                    // if (p == GrappleRider) return false;

                    // Debug.Break();
                    if (ridingActors.Contains(a)) {
                        ridingActors.Remove(a);
                    }
                    if (!allActors.Contains(a)) {
                        if (onCollide(a, d)) {
                            return true;
                        }
                    } else
                    {
                        //Push actors
                        a.Push(direction, this);
                    }

                    return false;
                });

                //Ride actors
                foreach (var a in ridingActors) {
                    a.Ride(direction);
                }
                
                if (collision) return true;
                
                transform.position += new Vector3((int)direction.x, (int)direction.y, 0);
                NextFrameOffset += new Vector2((int)direction.x, (int)direction.y);
                remainder -= 1;
            }
            
            return false;
        }

        public override bool Squish(PhysObj p, Vector2 d) {
            return false;
        }
    }
}