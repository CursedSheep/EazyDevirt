﻿using AsmResolver.DotNet.Serialized;
using AsmResolver.PE.DotNet.Cil;
using EazyDevirt.Core.Abstractions;
using EazyDevirt.Core.Abstractions.Interfaces;
using EazyDevirt.Core.Architecture;

namespace EazyDevirt.PatternMatching.Patterns.OpCodes;

internal record SetBranchIndexPattern : IPattern
{
    public IList<CilOpCode> Pattern => new List<CilOpCode>
    {
        CilOpCodes.Ldarg_0,     // 0	0000	ldarg.0
        CilOpCodes.Ldarg_1,     // 1	0001	ldarg.1
        CilOpCodes.Newobj,      // 2	0002	newobj	instance void valuetype [mscorlib]System.Nullable`1<uint32>::.ctor(!0)
        CilOpCodes.Stfld,       // 3	0007	stfld	valuetype [mscorlib]System.Nullable`1<uint32> VM::branchIndex
        CilOpCodes.Ret          // 4	000C	ret
    };
}

#region Br

internal record Br : IOpCodePattern
{
    public IList<CilOpCode> Pattern => new List<CilOpCode>
    {
        CilOpCodes.Ldarg_1,     // 0	0000	ldarg.1
        CilOpCodes.Castclass,   // 1	0001	castclass	VMUIntOperand
        CilOpCodes.Callvirt,    // 2	0006	callvirt	instance uint32 VMUIntOperand::method_3()
        CilOpCodes.Stloc_0,     // 3	000B	stloc.0
        CilOpCodes.Ldarg_0,     // 4	000C	ldarg.0
        CilOpCodes.Ldloc_0,     // 5	000D	ldloc.0
        CilOpCodes.Callvirt,    // 6	000E	callvirt	instance void VM::SetBranchIndex(uint32)
        CilOpCodes.Ret          // 7	0013	ret
    };

    public CilOpCode? CilOpCode => CilOpCodes.Br;

    public bool Verify(VMOpCode vmOpCode, int index) =>
        PatternMatcher.MatchesPattern(new SetBranchIndexPattern(),
            (vmOpCode.SerializedDelegateMethod.CilMethodBody?.Instructions[6].Operand as SerializedMethodDefinition)!);
}
#endregion Br

#region Bgt

internal record BgtInnerPattern : IPattern
{
    public IList<CilOpCode> Pattern => new List<CilOpCode>
    {
        CilOpCodes.Ldarg_0,     // 97	0102	ldarg.0
        CilOpCodes.Castclass,   // 98	0103	castclass	VMIntOperand
        CilOpCodes.Callvirt,    // 99	0108	callvirt	instance int32 VMIntOperand::method_3()
        CilOpCodes.Ldarg_1,     // 100	010D	ldarg.1
        CilOpCodes.Castclass,   // 101	010E	castclass	VMIntOperand
        CilOpCodes.Callvirt,    // 102	0113	callvirt	instance int32 VMIntOperand::method_3()
        CilOpCodes.Cgt,         // 103	0118	cgt
        CilOpCodes.Stloc_0,     // 104	011A	stloc.0
    };

    public bool MatchEntireBody => false;
    
    public bool InterchangeStlocOpCodes => true;
}

internal record Bgt : IOpCodePattern
{
    public IList<CilOpCode> Pattern => new List<CilOpCode>
    {
        CilOpCodes.Ldarg_0,     // 0	0000	ldarg.0
        CilOpCodes.Callvirt,    // 1	0001	callvirt	instance class VMOperandType VM::PopStack()
        CilOpCodes.Stloc_0,     // 2	0006	stloc.0
        CilOpCodes.Ldarg_0,     // 3	0007	ldarg.0
        CilOpCodes.Callvirt,    // 4	0008	callvirt	instance class VMOperandType VM::PopStack()
        CilOpCodes.Ldloc_0,     // 5	000D	ldloc.0
        CilOpCodes.Call,        // 6	000E	call	bool VM::BgtInner(class VMOperandType, class VMOperandType)
        CilOpCodes.Brfalse_S,   // 7	0013	brfalse.s	15 (0028) ret 
    };

    public CilOpCode? CilOpCode => CilOpCodes.Bgt;

    public bool MatchEntireBody => false;
    
    public bool InterchangeLdlocOpCodes => true;
    public bool InterchangeStlocOpCodes => true;

    public bool Verify(VMOpCode vmOpCode, int index) => PatternMatcher.MatchesPattern(new BgtInnerPattern(),
            (vmOpCode.SerializedDelegateMethod.CilMethodBody?.Instructions[6].Operand as SerializedMethodDefinition)!);
}
#endregion Bgt

#region Blt
internal record Blt : IOpCodePattern
{
    public IList<CilOpCode> Pattern => new List<CilOpCode>
    {
        CilOpCodes.Ldarg_0,     // 0	0000	ldarg.0
        CilOpCodes.Callvirt,    // 1	0001	callvirt	instance class VMOperandType VM::PopStack()
        CilOpCodes.Stloc_0,     // 2	0006	stloc.0
        CilOpCodes.Ldarg_0,     // 3	0007	ldarg.0
        CilOpCodes.Callvirt,    // 4	0008	callvirt	instance class VMOperandType VM::PopStack()
        CilOpCodes.Ldloc_0,     // 5	000D	ldloc.0
        CilOpCodes.Call,        // 6	000E	call	bool VM::CltInner(class VMOperandType, class VMOperandType)
        CilOpCodes.Brfalse_S,   // 7	0013	brfalse.s	15 (0028) ret 
    };

    public CilOpCode? CilOpCode => CilOpCodes.Blt;

    public bool MatchEntireBody => false;
    
    public bool InterchangeLdlocOpCodes => true;
    public bool InterchangeStlocOpCodes => true;

    public bool Verify(VMOpCode vmOpCode, int index) => PatternMatcher.MatchesPattern(new CltInnerPattern(),
        (vmOpCode.SerializedDelegateMethod.CilMethodBody?.Instructions[6].Operand as SerializedMethodDefinition)!);
}
#endregion Blt

#region Blt_Un
internal record Blt_Un : IOpCodePattern
{
    public IList<CilOpCode> Pattern => new List<CilOpCode>
    {
        CilOpCodes.Ldarg_0,     // 0	0000	ldarg.0
        CilOpCodes.Callvirt,    // 1	0001	callvirt	instance class VMOperandType VM::PopStack()
        CilOpCodes.Stloc_0,     // 2	0006	stloc.0
        CilOpCodes.Ldarg_0,     // 3	0007	ldarg.0
        CilOpCodes.Callvirt,    // 4	0008	callvirt	instance class VMOperandType VM::PopStack()
        CilOpCodes.Ldloc_0,     // 5	000D	ldloc.0
        CilOpCodes.Call,        // 6	000E	call	bool VM::Clt_UnInner(class VMOperandType, class VMOperandType)
        CilOpCodes.Brfalse_S,   // 7	0013	brfalse.s	15 (0028) ret 
    };

    public CilOpCode? CilOpCode => CilOpCodes.Blt_Un;

    public bool MatchEntireBody => false;
    
    public bool InterchangeLdlocOpCodes => true;
    public bool InterchangeStlocOpCodes => true;

    public bool Verify(VMOpCode vmOpCode, int index) => PatternMatcher.MatchesPattern(new Clt_UnInnerPattern(),
        (vmOpCode.SerializedDelegateMethod.CilMethodBody?.Instructions[6].Operand as SerializedMethodDefinition)!);
}
#endregion Blt_Un

#region Bge
internal record Bge : IOpCodePattern
{
    public IList<CilOpCode> Pattern => new List<CilOpCode>
    {
        CilOpCodes.Ldarg_0,     // 0	0000	ldarg.0
        CilOpCodes.Callvirt,    // 1	0001	callvirt	instance class VMOperandType VM::PopStack()
        CilOpCodes.Stloc_0,     // 2	0006	stloc.0
        CilOpCodes.Ldarg_0,     // 3	0007	ldarg.0
        CilOpCodes.Callvirt,    // 4	0008	callvirt	instance class VMOperandType VM::PopStack()
        CilOpCodes.Ldloc_0,     // 5	000D	ldloc.0
        CilOpCodes.Call,        // 6	000E	call	bool VM::CltInner(class VMOperandType, class VMOperandType)
        CilOpCodes.Brtrue_S,    // 7	0013	brtrue.s	15 (0028) ret 
    };

    public CilOpCode? CilOpCode => CilOpCodes.Bge;

    public bool MatchEntireBody => false;
    
    public bool InterchangeLdlocOpCodes => true;
    public bool InterchangeStlocOpCodes => true;

    public bool Verify(VMOpCode vmOpCode, int index) => PatternMatcher.MatchesPattern(new CltInnerPattern(),
        (vmOpCode.SerializedDelegateMethod.CilMethodBody?.Instructions[6].Operand as SerializedMethodDefinition)!);
}
#endregion Bge

#region Bge_Un
internal record Bge_Un : IOpCodePattern
{
    public IList<CilOpCode> Pattern => new List<CilOpCode>
    {
        CilOpCodes.Ldarg_0,     // 0	0000	ldarg.0
        CilOpCodes.Callvirt,    // 1	0001	callvirt	instance class VMOperandType VM::PopStack()
        CilOpCodes.Stloc_0,     // 2	0006	stloc.0
        CilOpCodes.Ldarg_0,     // 3	0007	ldarg.0
        CilOpCodes.Callvirt,    // 4	0008	callvirt	instance class VMOperandType VM::PopStack()
        CilOpCodes.Ldloc_0,     // 5	000D	ldloc.0
        CilOpCodes.Call,        // 6	000E	call	bool VM::Clt_UnInner(class VMOperandType, class VMOperandType)
        CilOpCodes.Brtrue_S,    // 7	0013	brtrue.s	15 (0028) ret 
    };

    public CilOpCode? CilOpCode => CilOpCodes.Bge_Un;

    public bool MatchEntireBody => false;
    
    public bool InterchangeLdlocOpCodes => true;
    public bool InterchangeStlocOpCodes => true;

    public bool Verify(VMOpCode vmOpCode, int index) => PatternMatcher.MatchesPattern(new Clt_UnInnerPattern(),
        (vmOpCode.SerializedDelegateMethod.CilMethodBody?.Instructions[6].Operand as SerializedMethodDefinition)!);
}
#endregion Bge_Un

#region Beq

internal record Beq : IOpCodePattern
{
    public IList<CilOpCode> Pattern => new List<CilOpCode>
    {
        CilOpCodes.Ldarg_0,     // 0	0000	ldarg.0
        CilOpCodes.Callvirt,    // 1	0001	callvirt	instance class VMOperandType VM::PopStack()
        CilOpCodes.Stloc_0,     // 2	0006	stloc.0
        CilOpCodes.Ldarg_0,     // 3	0007	ldarg.0
        CilOpCodes.Callvirt,    // 4	0008	callvirt	instance class VMOperandType VM::PopStack()
        CilOpCodes.Ldloc_0,     // 5	000D	ldloc.0
        CilOpCodes.Call,        // 6	000E	call	bool VM::CeqInner(class VMOperandType, class VMOperandType)
        CilOpCodes.Brfalse_S,   // 7	0013	brfalse.s	15 (0028) ret
    };

    public CilOpCode? CilOpCode => CilOpCodes.Beq;

    public bool MatchEntireBody => false;
    
    public bool InterchangeLdlocOpCodes => true;
    public bool InterchangeStlocOpCodes => true;

    public bool Verify(VMOpCode vmOpCode, int index) => PatternMatcher.MatchesPattern(new CeqInnerPattern(),
        (vmOpCode.SerializedDelegateMethod.CilMethodBody?.Instructions[6].Operand as SerializedMethodDefinition)!);
}
#endregion Beq

#region Bne_Un

internal record Bne_Un : IOpCodePattern
{
    public IList<CilOpCode> Pattern => new List<CilOpCode>
    {
        CilOpCodes.Ldarg_0,     // 0	0000	ldarg.0
        CilOpCodes.Callvirt,    // 1	0001	callvirt	instance class VMOperandType VM::PopStack()
        CilOpCodes.Stloc_0,     // 2	0006	stloc.0
        CilOpCodes.Ldarg_0,     // 3	0007	ldarg.0
        CilOpCodes.Callvirt,    // 4	0008	callvirt	instance class VMOperandType VM::PopStack()
        CilOpCodes.Ldloc_0,     // 5	000D	ldloc.0
        CilOpCodes.Call,        // 6	000E	call	bool VM::CeqInner(class VMOperandType, class VMOperandType)
        CilOpCodes.Brtrue_S,    // 7	0013	brtrue.s	15 (0028) ret
    };

    public CilOpCode? CilOpCode => CilOpCodes.Bne_Un;

    public bool MatchEntireBody => false;
    
    public bool InterchangeLdlocOpCodes => true;
    public bool InterchangeStlocOpCodes => true;

    public bool Verify(VMOpCode vmOpCode, int index) => PatternMatcher.MatchesPattern(new CeqInnerPattern(),
        (vmOpCode.SerializedDelegateMethod.CilMethodBody?.Instructions[6].Operand as SerializedMethodDefinition)!);
}
#endregion Bne_Un

#region Brtrue

internal record Brtrue : IOpCodePattern
{
    public IList<CilOpCode> Pattern => new List<CilOpCode>
    {
        CilOpCodes.Ldloc_0,     // 52	0097	ldloc.0
        CilOpCodes.Callvirt,    // 53	0098	callvirt	instance object VMOperandType::vmethod_0()
        CilOpCodes.Ldnull,      // 54	009D	ldnull
        CilOpCodes.Cgt_Un,      // 55	009E	cgt.un
        CilOpCodes.Stloc_1,     // 56	00A0	stloc.1
    };

    public CilOpCode? CilOpCode => CilOpCodes.Brtrue;

    public bool MatchEntireBody => false;
}
#endregion Brtrue

#region Brfalse

internal record Brfalse : IOpCodePattern
{
    public IList<CilOpCode> Pattern => new List<CilOpCode>
    {
        CilOpCodes.Ldloc_0,     // 54	009A	ldloc.0
        CilOpCodes.Callvirt,    // 55	009B	callvirt	instance object VMOperandType::vmethod_0()
        CilOpCodes.Ldnull,      // 56	00A0	ldnull
        CilOpCodes.Ceq,         // 57	00A1	ceq
        CilOpCodes.Stloc_1,     // 58	00A3	stloc.1
    };

    public CilOpCode? CilOpCode => CilOpCodes.Brfalse;

    public bool MatchEntireBody => false;
}
#endregion Brfalse

#region Switch

internal record Switch : IOpCodePattern
{
    public IList<CilOpCode> Pattern => new List<CilOpCode>
    {
        CilOpCodes.Ldlen,       // 41	005F	ldlen
        CilOpCodes.Conv_I4,     // 42	0060	conv.i4
        CilOpCodes.Conv_I8,     // 43	0061	conv.i8
        CilOpCodes.Blt_S,       // 44	0062	blt.s	46 (0065) ldloc.3 
        CilOpCodes.Ret,         // 45	0064	ret
        CilOpCodes.Ldloc_3,     // 46	0065	ldloc.3
        CilOpCodes.Ldloc_2,     // 47	0066	ldloc.2
        CilOpCodes.Ldelem,      // 48	0067	ldelem	VMIntOperand
        CilOpCodes.Callvirt,    // 49	006C	callvirt	instance int32 VMIntOperand::method_3()
        CilOpCodes.Stloc_S,     // 50	0071	stloc.s	V_4 (4)
        CilOpCodes.Ldarg_0,     // 51	0073	ldarg.0
        CilOpCodes.Ldloc_S,     // 52	0074	ldloc.s	V_4 (4)
        CilOpCodes.Callvirt,    // 53	0076	callvirt	instance void VM::SetBranchIndex(uint32)
        CilOpCodes.Ret,         // 54	007B	ret
    };

    public CilOpCode? CilOpCode => CilOpCodes.Switch;

    public bool MatchEntireBody => false;

    public bool InterchangeLdlocOpCodes => true;

    public bool Verify(VMOpCode vmOpCode, int index = 0) =>
        vmOpCode.SerializedDelegateMethod.CilMethodBody?.Instructions![index + 12].Operand is SerializedMethodDefinition
            setBranchIndexCall &&
        PatternMatcher.MatchesPattern(new SetBranchIndexPattern(), setBranchIndexCall);
}
#endregion Switch