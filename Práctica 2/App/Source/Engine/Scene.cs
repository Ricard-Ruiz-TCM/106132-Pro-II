using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace TcGame
{
    public class Scene : Drawable
    {
        private List<Actor> actors = new List<Actor>();
        private List<Actor> actorsToDestroy = new List<Actor>();
        private List<Actor> actorsToAdd = new List<Actor>();

        /// <summary>
        /// Adds and returns an actor of type T to the scene
        /// </summary>
        /// <typeparam name="T">The class that we want create the instance</typeparam>
        public T Create<T>() where T : Actor, new()
        {
            T actor = new T();
            actorsToAdd.Add(actor);
            return actor;
        }

        /// <summary>
        /// Adds and returns an actor of type T to the scene
        /// </summary>
        /// <param name="Position">Initial position where the actor will be placed in screen coords</param>
        /// <typeparam name="T">The class that we want create the instance</typeparam>
        public T Create<T>(Vector2f Position) where T : Actor, new()
        {
            T actor = new T();
            actor.Position = Position;
            actorsToAdd.Add(actor);
            return actor;
        }

        /// <summary>
        /// Removes the actor from the scene
        /// </summary>
        public void Destroy(Actor actor)
        {
            actorsToAdd.Remove(actor);
            actorsToDestroy.Add(actor);

            if (actor.OnDestroy != null)
            {
                actor.OnDestroy(actor);
            }
        }

        public void Update(float dt)
        {
            actors.RemoveAll(actorsToDestroy.Contains);
            actorsToDestroy.Clear();

            actors.AddRange(actorsToAdd);
            actorsToAdd.Clear();

            foreach (Actor actor in actors)
            {
                actor.Update(dt);
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            int NumLayers = Enum.GetValues(typeof(Actor.ELayer)).Length;
            for (int i = 0; i < NumLayers; ++i)
            {
                Actor.ELayer layer = (Actor.ELayer)i;

                List<Actor> actorsInLayer = actors.FindAll(x => x.Layer == layer);
                actorsInLayer.ForEach(x => target.Draw(x, states));
            }
        }

        /// <summary>
        /// Gets all actors of type T
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public List<T> GetAll<T>() where T : Actor
        {
            return actors.FindAll(x => !actorsToDestroy.Contains(x)).FindAll(x => x is T).ConvertAll(x => x as T);
        }

        /// <summary>
        /// Gets the first actor in scene of type T
        /// </summary>
        public T GetFirst<T>() where T : Actor
        {
            return actors.Find(x => x is T) as T;
        }

        /// <summary>
        /// Returns a random actor in scene of type T
        /// </summary>
        public T GetRandom<T>() where T : Actor
        {
            Random r = new Random();

            List<T> tActors = GetAll<T>();
            return (tActors.Count > 0) ? tActors[r.Next(tActors.Count)] : null;
        }
    }
}
