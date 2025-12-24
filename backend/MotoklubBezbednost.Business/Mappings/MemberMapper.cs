using System.Collections.Generic;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

/// <summary>
/// Concrete mapper for MemberDb &lt;-&gt; MemberDto following the generic mapper interfaces.
/// This wraps the existing extension methods in MemberMappings so it is easy to unit test or inject.
/// </summary>
public class MemberMapper : ITwoWayDbMapper<MemberDb, MemberDto>
{
    public MemberDto ToDto(MemberDb entity) => entity.ToDto();

    public IEnumerable<MemberDto> ToDto(IEnumerable<MemberDb> entities) => entities.ToDto();

    public MemberDb ToEntity(MemberDto dto, MemberDb? existing = null) => dto.ToEntity(existing);
}


