using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoEcommerce.Infrastructure
{
    public class MediaDeclare
    {
    }
    [ContentType(GUID = "EE3BD195-7CB0-4756-AB5F-E5E223CD9820")]
    public class GenericMedia : MediaData
    {
        public virtual string Description { get; set; }
    }

    [ContentType(GUID = "0A89E464-56D4-449F-AEA8-2BF774AB8730")]
    [MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png")]
    public class ImageFile : CommerceImage
    {
        public virtual string Description { get; set; }
        public virtual string Copyright { get; set; }
    }

    [ContentType(GUID = "85468104-E06F-47E5-A317-FC9B83D3CBA6")]
    [MediaDescriptor(ExtensionString = "flv,mp4,webm")]
    public class VideoFile : VideoData
    {
        public virtual string Description { get; set; }
    }
}