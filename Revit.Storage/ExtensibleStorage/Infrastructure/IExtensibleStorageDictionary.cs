﻿namespace Revit.Storage.ExtensibleStorage.Infrastructure;

public interface IExtensibleStorageDictionary : IExtensibleStorage
{
    string GetEntity(string key);

    void AddEntity(string key, string entity);

    void UpdateEntity(string key, string entity);

    bool RemoveEntity(string key);

    int GetNextId(string prefix);

    IEnumerable<string> GetAll<T>(string prefix);
}