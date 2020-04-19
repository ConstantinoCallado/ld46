using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


[CustomPropertyDrawer(typeof(Spawn))] 
public class SpawnDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Calculate rects
        float rectWidth = position.width / 4.0f - 15.0f;
        float currentPosition = position.x;
        Rect secondsLabelRect = new Rect(currentPosition, position.y, rectWidth, position.height);
        currentPosition += rectWidth + 5f;
        Rect secondsRect = new Rect(currentPosition, position.y, rectWidth, position.height);
        currentPosition += rectWidth + 5f;
        Rect dancersLabelRect = new Rect(currentPosition, position.y, rectWidth, position.height);
        currentPosition += rectWidth + 5f;
        Rect dancersRect = new Rect(currentPosition, position.y, rectWidth, position.height);

        EditorGUI.LabelField(secondsLabelRect, "Seconds");
        EditorGUI.PropertyField(secondsRect, property.FindPropertyRelative("seconds"), GUIContent.none);
        EditorGUI.LabelField(dancersLabelRect, "Dancers");
        EditorGUI.PropertyField(dancersRect, property.FindPropertyRelative("dancers"), GUIContent.none);

        EditorGUI.EndProperty();
    }
}