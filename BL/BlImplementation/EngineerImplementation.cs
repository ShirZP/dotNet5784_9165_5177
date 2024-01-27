namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;

    public void Create(BO.Engineer engineer)
    {
        //TODO: בדיקות תקינות קלט שדות וזריקת חריגות מתאימות

        DO.Engineer doEngineer = new DO.Engineer(engineer.ID, engineer.FullName, engineer.Email, engineer.Level, engineer.Cost);
        //TaskInEngineer //TODO: השדה הזה לא מוגדר בבנאי מתי צריך להוסיף אותו?
        try
        {

        }
        catch 
        {
        
        }
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Engineer Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Engineer UpdatedEngineer)
    {
        throw new NotImplementedException();
    }
}
