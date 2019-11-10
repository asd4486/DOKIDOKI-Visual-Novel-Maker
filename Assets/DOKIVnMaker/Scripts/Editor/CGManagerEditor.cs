using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace DokiVnMaker
{
    [CustomEditor(typeof(CGManagerObject))]
    public class CGManagerEditor : Editor
    {
        CGManagerObject cgManager;
        Vector2 scrollPosition;

        SerializedProperty cgListProperty;
        ReorderableList reorderableList;

        private void OnEnable()
        {
            cgManager = target as CGManagerObject;
            cgListProperty = serializedObject.FindProperty("CGList");

            //Create an instance of our reorderable list.
            reorderableList = new ReorderableList(serializedObject: serializedObject, elements: cgListProperty, draggable: true, displayHeader: true,
                displayAddButton: true, displayRemoveButton: true);

            //Set up the method callback to draw our list header
            reorderableList.drawHeaderCallback = DrawHeaderCallback;

            //Set up the method callback to draw each element in our reorderable list
            reorderableList.drawElementCallback = DrawElementCallback;

            //Set the height of each element.
            reorderableList.elementHeightCallback += ElementHeightCallback;

            //Set up the method callback to define what should happen when we add a new object to our list.
            reorderableList.onAddCallback += OnAddCallback;
            reorderableList.onChangedCallback += OnChangeCallback;
        }

        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, "CGs");
        }

        private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
        {
            //Get the element we want to draw from the list.
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            //We get the name property of our element so we can display this in our list.
            var elementName = element.FindPropertyRelative("name").stringValue;
            string elementTitle = $"{index}: {elementName}";

            //Draw the list item as a property field, just like Unity does internally.
            EditorGUI.PropertyField(position:
                new Rect(rect.x += 10, rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight), property:
                element, label: new GUIContent(elementTitle), includeChildren: true);
        }

        private float ElementHeightCallback(int index)
        {
            //Gets the height of the element. This also accounts for properties that can be expanded, like structs.
            float propertyHeight =
                EditorGUI.GetPropertyHeight(reorderableList.serializedProperty.GetArrayElementAtIndex(index), true);

            float spacing = EditorGUIUtility.singleLineHeight / 2;
            return propertyHeight + spacing;
        }

        private void OnAddCallback(ReorderableList list)
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("name").stringValue = $"CG_ {index}";
        }

        private void OnChangeCallback(ReorderableList list)
        {
            cgManager.SetupCgIndex();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}