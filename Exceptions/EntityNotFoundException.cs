namespace Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base($"Could not find Entity.") { }
        public EntityNotFoundException(int entityId) : base($"Could not find Entity with ID {entityId}.") { }
    }
}
