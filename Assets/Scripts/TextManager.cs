using System;
using Random = UnityEngine.Random;

public class TextManager : Singleton<TextManager>
{
    public String[] computerFixed;
    public String[] computerFixedEnemy;
    public String[] notCarryingMoreExcuse;
    public String[] tookComputerPart;
    public String[] tookKey;
    public String[] doorOpened;
    public String[] doorClosed;
    public String[] hiding;
    public String[] stoodUp;
    
    public String[] bossText;
    public String[] delegatorText;
    public String[] hottieText;
    public String[] bossProblem;
    public String[] delegatorProblem;
    public String[] hottieProblem;
    public String[] requiresPart;



    private String _noString = "No saved fixed strings, bro!";

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Random.InitState(this.GetInstanceID());
    }

    public String GetRequiresPart()
    {
        int count = requiresPart.Length;
        return count > 0 ? requiresPart[Random.Range(0, count)] : _noString;
    }

    public String GetBossProblem()
    {
        int count = bossProblem.Length;
        return count > 0 ? bossProblem[Random.Range(0, count)] : _noString;
    }
    
    public String GetDelegatorProblem()
    {
        int count = delegatorProblem.Length;
        return count > 0 ? delegatorProblem[Random.Range(0, count)] : _noString;
    }
    
    public String GetHottieProblem()
    {
        int count = hottieProblem.Length;
        return count > 0 ? hottieProblem[Random.Range(0, count)] : _noString;
    }
    
    public String GetBossText()
    {
        int count = bossText.Length;
        return count > 0 ? bossText[Random.Range(0, count)] : _noString;
    }
    
    public String GetDelegatorText()
    {
        int count = delegatorText.Length;
        return count > 0 ? delegatorText[Random.Range(0, count)] : _noString;
    }
    
    public String GetHottieText()
    {
        int count = hottieText.Length;
        return count > 0 ? hottieText[Random.Range(0, count)] : _noString;
    }
    
    public String GetStoodUpString()
    {
        int count = stoodUp.Length;
        return count > 0 ? stoodUp[Random.Range(0, count)] : _noString;
    }
    
    public String GetHidingString()
    {
        int count = hiding.Length;
        return count > 0 ? hiding[Random.Range(0, count)] : _noString;
    }
    
    public String GetDoorOpenedString()
    {
        int count = doorOpened.Length;
        return count > 0 ? doorOpened[Random.Range(0, count)] : _noString;
    }

    public String GetDoorLockedString()
    {
        int count = doorClosed.Length;
        return count > 0 ? doorClosed[Random.Range(0, count)] : _noString;
    }
    
    public String GetTookKeyString()
    {
        int count = tookKey.Length;
        return count > 0 ? tookKey[Random.Range(0, count)] : _noString;
    }
    
    public String GetTookComputerPartString()
    {
        int count = tookComputerPart.Length;
        return count > 0 ? tookComputerPart[Random.Range(0, count)] : _noString;
    }

    public String GetNotCarryingMoreString()
    {
        int count = notCarryingMoreExcuse.Length;
        return count > 0 ? notCarryingMoreExcuse[Random.Range(0, count)] : _noString;
    }

    public String GetComputerFixedString()
    {
        int count = computerFixed.Length;
        if (count > 0)
        {
            return computerFixed[Random.Range(0, count)];
        }
        else
        {
            return _noString;
        }
    }
    
    public String GetComputerFixedEnemyString()
    {
        int count = computerFixedEnemy.Length;
        if (count > 0)
        {
            return computerFixedEnemy[Random.Range(0, count)];
        }
        else
        {
            return _noString;
        }
    }
    
}
