
public class Stat
{
    public string StatName { get; private set; }
    public int BaseStatValue { get; private set; }
    public int MaxStatValue { get; private set; }
    public int CurrentStatValue { get; private set; }
    public bool IsModified { get; private set; }


    public Stat(string statName, int maxStatValue)
    {
        StatName = statName;
        MaxStatValue = maxStatValue;
        BaseStatValue = maxStatValue;
        CurrentStatValue = maxStatValue;
    }

    public void ResetMaxValue()
    {
        MaxStatValue = BaseStatValue;
    }

    public void ResetCurrentValue()
    {
        CurrentStatValue = MaxStatValue;
    }

    public void ModifyMaxValue(int value, Operator op)
    {
        switch (op)
        {
            case Operator.Add:
                MaxStatValue += value;
                break;

            case Operator.Minus:
                MaxStatValue -= value;

                if (MaxStatValue <= 0)
                    MaxStatValue = 0;

                break;

            case Operator.Multiply:
                MaxStatValue *= value;
                break;

            case Operator.Divide:
                MaxStatValue /= value;
                break;
        }
    }

    public void ModifyCurrentValue(int value, Operator op)
    {
        switch (op)
        {
            case Operator.Add:
                CurrentStatValue += value;

                if (CurrentStatValue >= MaxStatValue)
                    CurrentStatValue = MaxStatValue;

                break;

            case Operator.Minus:
                CurrentStatValue -= value;

                if (CurrentStatValue < 0)
                    CurrentStatValue = 0;

                break;

            case Operator.Multiply:
                CurrentStatValue *= value;

                if (CurrentStatValue >= MaxStatValue)
                    CurrentStatValue = MaxStatValue;

                break;

            case Operator.Divide:
                CurrentStatValue /= value;
                break;
        }
    }
}
