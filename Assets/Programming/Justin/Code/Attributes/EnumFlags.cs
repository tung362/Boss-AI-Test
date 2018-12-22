using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class EnumFlagsAttribute : PropertyAttribute
{ public EnumFlagsAttribute() { } }

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        EditorGUI.showMixedValue = _property.hasMultipleDifferentValues;
        _property.intValue = EditorGUI.MaskField(_position, _label, _property.intValue, _property.enumNames);
    }
}
#endif