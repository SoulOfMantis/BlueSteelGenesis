using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Класс ключевых слов исключительно для внутреннего использования
/// </summary>
public abstract class InternalKeyword : ModuleKeyword {}
public class DefenseKeyword : InternalKeyword {}
public class OffenseKeyword : InternalKeyword {}
public class MobilityKeyword : InternalKeyword { }
public class CommonKeyword : InternalKeyword {}
public class BossKeyword : InternalKeyword {}
public class AdaptiveKeyword : InternalKeyword {}
public class ActiveKeyword : InternalKeyword {}
public class PassiveKeyword : InternalKeyword {}
public class StatusKeyword : InternalKeyword { }
//TODO: create more keywords
