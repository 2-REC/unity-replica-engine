using System;

namespace ReplicaEngine {

    public class GameComponentPool : TEntityPool<GameComponent> {
        //public Class<?> entityClass;
        public Type entityClass;


        // TODO: make sure it calls base ctor
        //public GameComponentPool(Class<?> type) {
        public GameComponentPool(Type type) {
            entityClass = type;
            // TODO: fill is called twice...
            Fill();
        }

        //public GameComponentPool(Class<?> type, int size) {
        public GameComponentPool(Type type, int size) : base(size) {
            entityClass = type;
            // TODO: fill is called twice...
            Fill();
        }

        /*
        public static GameComponentPool CreateComponent(GameObject targetObject, Type type, int size) {
            GameComponentPool component = targetObject.AddComponent<GameComponentPool>();
            component.entityClass = type;
            return component;
        }
        */

        // TODO: 'object' ok? (if not, try with 'GameComponent' and see about derived classes)
        protected override object CreateItem() {
            return Activator.CreateInstance(entityClass);
        }

    }

}
