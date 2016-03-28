//using UnityEngine;
//using UnityEditor;
//using System.Collections;


//[CustomEditor(typeof(MonsterController))]
//public class HandleTest : Editor {


//        Vector3 position = Vector3.zero;

//    void OnSceneGUI()
//    {        
        
//        Handles.PositionHandle(Vector3.zero, Quaternion.identity);

//        int controlID = GUIUtility.GetControlID(FocusType.Passive);
//        Debug.Log(Event.current.GetTypeForControl(controlID));
//        switch (Event.current.GetTypeForControl(controlID))
//        {
        
//        }

//        /*
//        MyHandles.DragHandleResult dhResult;
//        Vector3 newPosition = MyHandles.DragHandle(position, 1.0f, Handles.SphereCap, Color.red, out dhResult);
//        position = newPosition;
//        Debug.Log(GUIUtility.hotControl != 0);

//        switch (dhResult)
//        {
//            case MyHandles.DragHandleResult.LMBDoubleClick:
//                // do something

//                break;
//        }
//         * */
//    }


//    private static Vector3 DoPositionHandle_Internal(Vector3 position, Quaternion rotation)
//    {
//        float handleSize = HandleUtility.GetHandleSize(position);
//        Color color = Handles.color;
//        bool flag = !Tools.s_Hidden && EditorApplication.isPlaying && GameObjectUtility.ContainsStatic(Selection.gameObjects);
//        Handles.color = ((!flag) ? xAxisColor : Color.Lerp(xAxisColor, staticColor, staticBlend));
//        GUI.SetNextControlName("xAxis");
//        position = Handles.Slider(position, rotation * Vector3.right, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), EditorPrefs.GetFloat("MoveSnapX"));
//        Handles.color = ((!flag) ? yAxisColor : Color.Lerp(yAxisColor, staticColor, staticBlend));
//        GUI.SetNextControlName("yAxis");
//        position = Handles.Slider(position, rotation * Vector3.up, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), EditorPrefs.GetFloat("MoveSnapY"));
//        Handles.color = ((!flag) ? zAxisColor : Color.Lerp(zAxisColor, staticColor, staticBlend));
//        GUI.SetNextControlName("zAxis");
//        position = Handles.Slider(position, rotation * Vector3.forward, handleSize, new Handles.DrawCapFunction(Handles.ArrowCap), EditorPrefs.GetFloat("MoveSnapZ"));
//        //if (Handles.s_FreeMoveMode)
//        //{
//        //    Handles.color = Handles.centerColor;
//        //    GUI.SetNextControlName("FreeMoveAxis");
//        //    position = Handles.FreeMoveHandle(position, rotation, handleSize * 0.15f, SnapSettings.move, new Handles.DrawCapFunction(Handles.RectangleCap));
//        //}
//        //else
//        {
//            position = DoPlanarHandle(PlaneHandle.xzPlane, position, rotation, handleSize * 0.25f);
//            position = DoPlanarHandle(PlaneHandle.xyPlane, position, rotation, handleSize * 0.25f);
//            position = DoPlanarHandle(PlaneHandle.yzPlane, position, rotation, handleSize * 0.25f);
//        }
//        Handles.color = color;
//        return position;
//    }
//    static int s_xzAxisMoveHandleHash = "xzAxisFreeMoveHandleHash".GetHashCode();
//    static int s_xyAxisMoveHandleHash = "xyAxisFreeMoveHandleHash".GetHashCode();
//    static int s_yzAxisMoveHandleHash = "yzAxisFreeMoveHandleHash".GetHashCode();
//    private enum PlaneHandle
//    {
//        xzPlane,
//        xyPlane,
//        yzPlane
//    }

//    private static bool currentlyDragging
//    {
//        get
//        {
//            return GUIUtility.hotControl != 0;
//        }
//    }

//    private static Vector3 s_PlanarHandlesOctant = Vector3.one;
//    static Color staticColor = new Color(0.5f, 0.5f, 0.5f, 0f);
//    static float staticBlend = 0.6f;
//    internal static Color xAxisColor = new Color(0.858823538f, 0.243137255f, 0.113725491f, 0.93f);
//    internal static Color yAxisColor = new Color(0.6039216f, 0.9529412f, 0.282352954f, 0.93f);
//    internal static Color zAxisColor = new Color(0.227450982f, 0.478431374f, 0.972549f, 0.93f);
//    private static Vector3 DoPlanarHandle(PlaneHandle planeID, Vector3 position, Quaternion rotation, float handleSize)
//    {
//        int num = 0;
//        int num2 = 0;
//        int hint = 0;
//        bool flag = !Tools.s_Hidden && EditorApplication.isPlaying && GameObjectUtility.ContainsStatic(Selection.gameObjects);
//        switch (planeID)
//        {
//            case PlaneHandle.xzPlane:
//                num = 0;
//                num2 = 2;
//                Handles.color = ((!flag) ? yAxisColor : staticColor);
//                hint = s_xzAxisMoveHandleHash;
//                break;
//            case PlaneHandle.xyPlane:
//                num = 0;
//                num2 = 1;
//                Handles.color = ((!flag) ? zAxisColor : staticColor);
//                hint = s_xyAxisMoveHandleHash;
//                break;
//            case PlaneHandle.yzPlane:
//                num = 1;
//                num2 = 2;
//                Handles.color = ((!flag) ? xAxisColor : staticColor);
//                hint = s_yzAxisMoveHandleHash;
//                break;
//        }
//        int index = 3 - num2 - num;
//        Color color = Handles.color;
//        Matrix4x4 matrix4x = Matrix4x4.TRS(position, rotation, Vector3.one);
//        Vector3 normalized;
//        if (Camera.current.orthographic)
//        {
//            normalized = matrix4x.inverse.MultiplyVector(SceneView.currentDrawingSceneView.cameraTargetRotation * -Vector3.forward).normalized;
//        }
//        else
//        {
//            normalized = matrix4x.inverse.MultiplyPoint(SceneView.currentDrawingSceneView.camera.transform.position).normalized;
//        }
//        int controlID = GUIUtility.GetControlID(hint, FocusType.Keyboard);
//        if (Mathf.Abs(normalized[index]) < 0.05f && GUIUtility.hotControl != controlID)
//        {
//            Handles.color = color;
//            return position;
//        }
//        if (!currentlyDragging)
//        {
//            s_PlanarHandlesOctant[num] = (float)((normalized[num] >= -0.01f) ? 1 : -1);
//            s_PlanarHandlesOctant[num2] = (float)((normalized[num2] >= -0.01f) ? 1 : -1);
//        }
//        Vector3 vector = s_PlanarHandlesOctant;
//        vector[index] = 0f;
//        vector = rotation * (vector * handleSize * 0.5f);
//        Vector3 vector2 = Vector3.zero;
//        Vector3 vector3 = Vector3.zero;
//        Vector3 vector4 = Vector3.zero;
//        vector2[num] = 1f;
//        vector3[num2] = 1f;
//        vector4[index] = 1f;
//        vector2 = rotation * vector2;
//        vector3 = rotation * vector3;
//        vector4 = rotation * vector4;
//        verts[0] = position + vector + (vector2 + vector3) * handleSize * 0.5f;
//        verts[1] = position + vector + (-vector2 + vector3) * handleSize * 0.5f;
//        verts[2] = position + vector + (-vector2 - vector3) * handleSize * 0.5f;
//        verts[3] = position + vector + (vector2 - vector3) * handleSize * 0.5f;
//        Handles.DrawSolidRectangleWithOutline(verts, new Color(Handles.color.r, Handles.color.g, Handles.color.b, 0.1f), new Color(0f, 0f, 0f, 0f));
//        position = Handles.Slider2D(controlID, position, vector, vector4, vector2, vector3, handleSize * 0.5f, new Handles.DrawCapFunction(Handles.RectangleCap), new Vector2(SnapSettings.move[num], SnapSettings.move[num2]));
//        Handles.color = color;
//        return position;
//    }

//    private static Vector3[] verts = new Vector3[4];

//}


