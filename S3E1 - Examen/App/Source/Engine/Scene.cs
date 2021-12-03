using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;

namespace TcGame
{
    public class Scene : Drawable
    {
        private List<Actor> actors = new List<Actor>();
        private List<Actor> actorsToDestroy = new List<Actor>();
        private List<Actor> actorsToAdd = new List<Actor>();

        private enum State
        {
            Waiting,
            Playing,
            GameOver
        }

        private State state = State.Waiting;

        public T Create<T>(Actor Parent = null) where T : Actor
        {
            return Create(typeof(T), Parent) as T;
        }

        public object Create(Type type, Actor Parent = null)
        {
            Actor actor = null;

            if (type.IsSubclassOf(typeof(Actor)))
            {
                actor = Activator.CreateInstance(type) as Actor;
                actor.Parent = Parent;
                actor.FixParent();

                actorsToAdd.Add(actor);
            }

            return actor;
        }

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

            actorsToDestroy.ForEach(x =>
              {
                  x.Parent = null;
                  x.FixParent();
              });
            actors.RemoveAll(actorsToDestroy.Contains);

            actors.AddRange(actorsToAdd);
            actorsToAdd.Clear();

            actors.ForEach(x => x.FixParent());

            List<Actor> roots = actors.FindAll(x => x.Parent == null);
            roots.ForEach(x => x.UpdateRecursive(dt));
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            int NumLayers = Enum.GetValues(typeof(Actor.ELayer)).Length;
            for (int i = 0; i < NumLayers; ++i)
            {
                Actor.ELayer layer = (Actor.ELayer)i;

                List<Actor> rootActorsInLayer = actors.FindAll(x => x.Layer == layer && x.Parent == null);
                rootActorsInLayer.ForEach(x => x.DrawRecursive(target, states));
            }

            //List<Actor> roots = actors.FindAll(x => x.Parent == null);
            //roots.ForEach(x => x.DrawRecursive(target, states));
        }


        public List<T> GetAll<T>() where T : Actor
        {
            return actors.FindAll(x => !actorsToDestroy.Contains(x)).FindAll(x => x is T).ConvertAll(x => x as T);
        }

        /// <summary>
        /// Returns the first actor in scene of type T, or null if it doesn't exist
        /// </summary>
        public T GetFirst<T>() where T : Actor
        {
            return actors.Find(x => x is T) as T;
        }

        public T GetPos<T>(Vector2f pos) where T : Actor
        {
            return actors.Find(x => (x is T)) as T;
            //&& ((x.Position - pos).Size() < 30));
            // System.Console.WriteLine(x.Position);
            //  System.Console.WriteLine(pos);

        }

        /// <summary>
        /// Returns an actor of type T, or null if it doesn't exist
        /// </summary>
        public T GetRandom<T>() where T : Actor
        {
            Random r = new Random();

            List<T> tActors = GetAll<T>();
            return (tActors.Count > 0) ? tActors[r.Next(tActors.Count)] : null;
        }
    }
}
