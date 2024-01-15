namespace SpecFlowTest
{
    public static class Config
    {
        public static int CircuitOpenTimeout => 4000;
        public static int CircuitClosedErrorLimit = 20;
        public static int CircuitHalfOpenSuccessLimit = 20;
    }
}