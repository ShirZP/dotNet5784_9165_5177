﻿using DO;
namespace DalApi;

public interface ICrud<T> where T : class  
{
    int Create(T item); //Creates new entity object in DAL
    T? Read(Func<T, bool> filter); //Reads entity object by filter 
    IEnumerable<T> ReadAll(Func<T, bool>? filter = null); //Reads all entities objects by filter 
    void Update(T item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
    void Clear(); //Clear all the objects from the data
}
