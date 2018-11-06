using Graphene.AutoMobileDynamics.Physics;
using UnityEditor;
using UnityEngine;

namespace Graphene.AutoMobileDynamics
{
    [CustomEditor(typeof(Car))]
    public class CarInspector : Editor
    {
        private Car _self;

        private bool _editAxes;

        private void Awake()
        {
            _self = (Car) target;

            if (_self.Physics == null)
                _self.Physics = new AutoPhysics();

            _self.Physics.CarMass = _self.GetComponent<BoxCollider>();
        }

        private void OnSceneGUI()
        {
            if(_editAxes)
            DrawAxis();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CarAxesSetup();
        }

        private void CarAxesSetup()
        {
            CheckAxes();

            _editAxes = EditorGUILayout.Toggle("Edit Axes:", _editAxes);
            
            if(!_editAxes) return;
            
            var w = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 18;
            for (int i = 0; i < _self.Physics.Axes.Length; i += 2)
            {
                EditorGUILayout.LabelField(i == 0 ? "Front" : "Back");
                EditorGUILayout.BeginHorizontal();
                _self.Physics.Axes[i] = EditorGUILayout.Vector3Field("R:", _self.Physics.Axes[i]);
                _self.Physics.Axes[i + 1] = EditorGUILayout.Vector3Field("L:", _self.Physics.Axes[i + 1]);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUIUtility.labelWidth = w;
        }

        private void CheckAxes()
        {
            if (_self.Physics.Axes == null || _self.Physics.Axes.Length <= 0)
            {
                _self.Physics.Axes = new Vector3[4];
            }
        }

        private void DrawAxis()
        {
            CheckAxes();

            for (int i = 0; i < _self.Physics.Axes.Length; i += 2)
            {
                EditorGUI.BeginChangeCheck();
                var axis = Handles.DoPositionHandle(_self.transform.TransformPoint(_self.Physics.Axes[i]), _self.transform.rotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "AxisMoved");

                    _self.Physics.Axes[i] = _self.transform.InverseTransformPoint(axis);
                    _self.Physics.Axes[i].x = Mathf.Abs(_self.Physics.Axes[i].x);
                    _self.Physics.Axes[i + 1] = new Vector3(-_self.Physics.Axes[i].x, _self.Physics.Axes[i].y, _self.Physics.Axes[i].z);
                }
                Handles.DrawWireDisc(_self.transform.TransformPoint(_self.Physics.Axes[i]), Vector3.right, i == 0 ? _self.Physics.FrontWheelSize : _self.Physics.BackWheelSize);
                Handles.DrawWireDisc(_self.transform.TransformPoint(_self.Physics.Axes[i + 1]), Vector3.right, i == 0 ? _self.Physics.FrontWheelSize : _self.Physics.BackWheelSize);
            }
        }
    }
}