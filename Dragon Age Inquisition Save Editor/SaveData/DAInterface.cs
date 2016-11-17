#region

using Dragon_Age_Inquisition_Save_Editor.DAIO;

#endregion

namespace Dragon_Age_Inquisition_Save_Editor.SaveData
{
    internal interface DAInterface<out T>
    {
        int Length { get; }
        T Read(DAIIO io);
        bool Write(DAIIO io, bool skiplength = false);
    }
}