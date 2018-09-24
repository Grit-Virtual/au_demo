using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Categories : MonoBehaviour {

    public static Categories instance;

    public HashSet<string> categories = new HashSet<string>();
    
    void Awake() {
        instance = this;
    }

    //Add to main list without duplication by using hashset
    public void AddCategoryList(List<string> newCategories) {
        if(newCategories.Count > 0) categories.Add(newCategories[0]);
    }

    public List<string> FilterCategories(string input) {
        List<string> returnList = new List<string>(categories);
        if (input == "") {
            return returnList;
        } else {
            return returnList.FindAll(cat => {
                return cat.ToLower().Contains(input.ToLower());
            });
        }
    }
}
