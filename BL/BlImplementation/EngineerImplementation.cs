namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;

    /// <summary>
    ///The function recieves an object of type BO.Engineer, Checks the correctness of fields and adds the engineer to the data layer as DO.Engineer.
    /// </summary>
    /// <param name="engineer">An object of type BO.Engineer</param>
    /// <exception cref="BlPositiveIntException">If the number is negative or equal to zero, throw an exception.</exception>
    /// <exception cref="BlEmptyStringException">If the string is empty throw an exception.</exception>
    /// <exception cref="BlAlreadyExistsException">If the engineer already exists throw an exception from the data layer.</exception>
    public int Create(BO.Engineer engineer)
    {     
        //TaskInEngineer //TODO: השדה הזה לא מוגדר בבנאי מתי צריך להוסיף אותו?
        try
        {
            //validation of the engineer's fields

            if(engineer.ID <= 0)
            {
                throw new BlPositiveIntException("The engineer's ID number must be positive!");
            }

            if(engineer.FullName == null)
            {
                throw new BlEmptyStringException("The engineer's full name can't be empty!");
            }

            if (engineer.Cost <= 0)
            {
                throw new BlPositiveIntException("The engineer's salary must be positive!");
            }

            if(engineer.Email == null) 
            {
                throw new BlEmptyStringException("The engineer's email can't be empty!");
            }

            //create dal engineer
            DO.Engineer doEngineer = new DO.Engineer(engineer.ID, engineer.FullName, engineer.Email, engineer.Level, engineer.Cost);

            //add the dal engineer to data layer
            int idEngineer = _dal.Engineer.Create(doEngineer);

            return idEngineer;
        }

        catch (DO.DalAlreadyExistsException dalEx)
        {
            throw new BlAlreadyExistsException($"An object of type Engineer with ID={engineer.ID} already exist", dalEx);
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
