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

                HashSet<PhysObj> ridingActors = new HashSet<PhysObj>(GetRidingActors(allActors));
                bool collision = CheckCollisions(direction, (p, d) => {
                    if (p == this) return false;
                    // if (p == GrappleRider) return false;

                    // Debug.Break();
                    if (ridingActors.Contains(p)) {
                        ridingActors.Remove(p);
                    }
                    if (!allActors.Contains(p)) {
                        if (onCollide(p, d)) {
                            return true;
                        }
                    } else
                    {
                        //Push actors
                        p.MoveGeneral(direction, 1, (ps, ds) => {
                            if (ps != this) return p.Squish(ps, ds);
                            return false;
                        });
                    }

                    return false;
                });

                //Ride actors
                foreach (var a in ridingActors) {
                    //Might cause a bug due to a being a PhysObj and GrappleRider is an actor
                    a.Ride(direction);
                }
                
                if (collision) return true;
                
                transform.position += new Vector3((int)direction.x, (int)direction.y, 0);
                NextFrameOffset += new Vector2((int)direction.x, (int)direction.y);
                remainder -= 1;
            }
            
            return false;
        }

        public HashSet<Actor> GetRidingActors(Actor[] allActors) {
            return new HashSet<Actor>(allActors.Where(c => c.IsRiding(this)));
        }

        public override bool Squish(PhysObj p, Vector2 d) {
            return false;
        }
    }
}