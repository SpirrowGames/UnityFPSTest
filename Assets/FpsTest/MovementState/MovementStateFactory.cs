namespace FpsTest
{
    public class MovementStateFactory
    {
        private readonly MovementStates _context;

        public MovementStateFactory(MovementStates currentContext) { _context = currentContext; }

        public IMovementState Default() { return new MovementDefaultState(_context, this); }

        public IMovementState Jump() { return new MovementJumpState(_context, this); }
        
        public IMovementState Die()
        {
            return null; 
            // 例えば以下のように death の状態を作る
            // return new PlayerDeadState(_context, this); 
        }

    }
}