// Copyright (C) 2016 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

// ========================================================================
// The code in this file is derived from the 'HEVCESBrowser' project,
// a tool for analyzing HEVC(h265) bitstreams authored by 'virinext'.
// See https://github.com/virinext/hevcesbrowser
// and http://www.codeproject.com/Tips/896030/The-Structure-of-HEVC-Video
// Licensed under the GNU General Public License and 
// the Code Project Open License, http://www.codeproject.com/info/cpol10.aspx
// ========================================================================

#ifndef HEVC_H_
#define HEVC_H_

//#include <windows.h>
#include <memory>
#include <vector>
#include <cstdint>
#include <cstddef>
#include <array>

namespace HEVC
{

	struct hevchdr
	{
		uint8_t  profile, level;
		uint64_t chromaFormat;
		uint16_t lumaDepth, chromaDepth;
		unsigned int width, height;
		bool progressive;
		uint8_t * sps;
		uint8_t * pps;
		uint8_t * vps;
		__int64 spslen;
		__int64 ppslen;
		__int64 vpslen;
		__int64 AvgTimePerFrame;
		int arx, ary;
		uint8_t ar;
		hevchdr()
		{
      profile = 0;
      level = 0;        
      chromaFormat = 0;          
      lumaDepth = 0;
      chromaDepth = 0;
			progressive = true;
		  sps = NULL;
		  pps = NULL;
		  vps = NULL;
			spslen = 0;
			ppslen = 0;
			vpslen = 0;
			AvgTimePerFrame = 370000;  //27 Hz
			ar = 0;
			arx = 0;
			ary = 0;
			width = 0;
			height = 0;
		}
		
		~hevchdr()
		{
		  if (sps != NULL) free(sps);
		  if (pps != NULL) free(pps);
		  if (vps != NULL) free(vps);
		  sps = NULL;
		  pps = NULL;
		  vps = NULL;
		}
	};

  enum NALUnitType 
  {
  	NAL_FAIL       = -1,
    NAL_TRAIL_N    = 0,
    NAL_TRAIL_R    = 1,
    NAL_TSA_N      = 2,
    NAL_TSA_R      = 3,
    NAL_STSA_N     = 4,
    NAL_STSA_R     = 5,
    NAL_RADL_N     = 6,
    NAL_RADL_R     = 7,
    NAL_RASL_N     = 8,
    NAL_RASL_R     = 9,
    NAL_BLA_W_LP   = 16,
    NAL_BLA_W_RADL = 17,
    NAL_BLA_N_LP   = 18,
    NAL_IDR_W_RADL = 19,
    NAL_IDR_N_LP   = 20,
    NAL_CRA_NUT    = 21,
    NAL_IRAP_VCL23 = 23, 
    NAL_VPS        = 32,
    NAL_SPS        = 33,
    NAL_PPS        = 34,
    NAL_AUD        = 35,
    NAL_EOS_NUT    = 36,
    NAL_EOB_NUT    = 37,
    NAL_FD_NUT     = 38,
    NAL_SEI_PREFIX = 39,
    NAL_SEI_SUFFIX = 40,
    NAL_RESERVED   = 255
  };

  class ProfileTierLevel
  {
  public:
    uint8_t                general_profile_space;
    uint8_t                general_tier_flag;
    uint8_t                general_profile_idc;
    uint8_t                general_profile_compatibility_flag[32];
    uint8_t                general_progressive_source_flag;
    uint8_t                general_interlaced_source_flag;
    uint8_t                general_non_packed_constraint_flag;
    uint8_t                general_frame_only_constraint_flag;
    uint8_t                general_level_idc;
    std::vector<uint8_t>   sub_layer_profile_present_flag;
    std::vector<uint8_t>   sub_layer_level_present_flag;
    std::vector<uint8_t>   sub_layer_profile_space;
    std::vector<uint8_t>   sub_layer_tier_flag;
    std::vector<uint8_t>   sub_layer_profile_idc;
    std::vector< std::vector< uint8_t> > 
                           sub_layer_profile_compatibility_flag;
    std::vector<uint8_t>   sub_layer_progressive_source_flag;
    std::vector<uint8_t>   sub_layer_interlaced_source_flag;
    std::vector<uint8_t>   sub_layer_non_packed_constraint_flag;
    std::vector<uint8_t>   sub_layer_frame_only_constraint_flag;
    std::vector<uint8_t>   sub_layer_level_idc;

    void toDefault();

    //bool operator == (const ProfileTierLevel &) const;
  };

  class SubLayerHrdParameters
  {
  public:    
    std::vector<uint32_t>       bit_rate_value_minus1;
    std::vector<uint32_t>       cpb_size_value_minus1;
    std::vector<uint32_t>       cpb_size_du_value_minus1;
    std::vector<uint32_t>       bit_rate_du_value_minus1;
    std::vector<uint8_t>        cbr_flag;

    void toDefault();

    //bool operator == (const SubLayerHrdParameters &) const;
  };


  class ScalingListData
  {
  public:
    std::vector< std::vector< uint8_t> >    scaling_list_pred_mode_flag;
    std::vector< std::vector< uint32_t> >   scaling_list_pred_matrix_id_delta;
    std::vector< std::vector< uint32_t> >   scaling_list_dc_coef_minus8;
    std::vector<std::vector< std::vector< uint32_t> > >
                                            scaling_list_delta_coef;

    void toDefault();

    //bool operator == (const ScalingListData &) const;
  };

  class HrdParameters
  {
  public:    
    uint8_t               nal_hrd_parameters_present_flag;
    uint8_t               vcl_hrd_parameters_present_flag;
    uint8_t               sub_pic_hrd_params_present_flag;
    uint8_t               tick_divisor_minus2;
    uint8_t               du_cpb_removal_delay_increment_length_minus1;
    uint8_t               sub_pic_cpb_params_in_pic_timing_sei_flag;
    uint8_t               dpb_output_delay_du_length_minus1;
    uint8_t               bit_rate_scale;
    uint8_t               cpb_size_scale;
    uint8_t               cpb_size_du_scale;
    uint8_t               initial_cpb_removal_delay_length_minus1;
    uint8_t               au_cpb_removal_delay_length_minus1;
    uint8_t               dpb_output_delay_length_minus1;
    std::vector<uint8_t>  fixed_pic_rate_general_flag;
    std::vector<uint8_t>  fixed_pic_rate_within_cvs_flag;
    std::vector<uint32_t> elemental_duration_in_tc_minus1;
    std::vector<uint8_t>  low_delay_hrd_flag;
    std::vector<uint32_t> cpb_cnt_minus1;
    std::vector<SubLayerHrdParameters> 
                          nal_sub_layer_hrd_parameters;
    std::vector<SubLayerHrdParameters> 
                          vcl_sub_layer_hrd_parameters;

    void toDefault();

    //bool operator == (const HrdParameters &) const;
 };

  class ShortTermRefPicSet
  {
  public:    
    uint8_t                   inter_ref_pic_set_prediction_flag;
    uint32_t                  delta_idx_minus1;
    uint8_t                   delta_rps_sign;
    uint32_t                  abs_delta_rps_minus1;
    std::vector<uint8_t>      used_by_curr_pic_flag;
    std::vector<uint8_t>      use_delta_flag;
    uint32_t                  num_negative_pics;
    uint32_t                  num_positive_pics;
    std::vector<uint32_t>     delta_poc_s0_minus1;
    std::vector<uint8_t>      used_by_curr_pic_s0_flag;
    std::vector<uint32_t>     delta_poc_s1_minus1;
    std::vector<uint8_t>      used_by_curr_pic_s1_flag;

    void toDefault();

    //bool operator == (const ShortTermRefPicSet &) const;    
  };
  
  class RefPicListModification
  {
  public:
    uint8_t                ref_pic_list_modification_flag_l0;
    std::vector<uint32_t>  list_entry_l0;
    uint8_t                ref_pic_list_modification_flag_l1;
    std::vector<uint32_t>  list_entry_l1;

    void toDefault();

    //bool operator == (const RefPicListModification &) const;
  };

  class VuiParameters
  {
  public:    
    uint8_t          aspect_ratio_info_present_flag;
    uint8_t          aspect_ratio_idc;
    uint16_t         sar_width;
    uint16_t         sar_height;
    uint8_t          overscan_info_present_flag;
    uint8_t          overscan_appropriate_flag;
    uint8_t          video_signal_type_present_flag;
    uint8_t          video_format;
    uint8_t          video_full_range_flag;
    uint8_t          colour_description_present_flag;
    uint8_t          colour_primaries;
    uint8_t          transfer_characteristics;
    uint8_t          matrix_coeffs;
    uint8_t          chroma_loc_info_present_flag;
    uint32_t         chroma_sample_loc_type_top_field;
    uint32_t         chroma_sample_loc_type_bottom_field;
    uint8_t          neutral_chroma_indication_flag;
    uint8_t          field_seq_flag;
    uint8_t          frame_field_info_present_flag;
    uint8_t          default_display_window_flag;
    uint32_t         def_disp_win_left_offset;
    uint32_t         def_disp_win_right_offset;
    uint32_t         def_disp_win_top_offset;
    uint32_t         def_disp_win_bottom_offset;
    uint8_t          vui_timing_info_present_flag;
    uint32_t         vui_num_units_in_tick;
    uint32_t         vui_time_scale;
    uint8_t          vui_poc_proportional_to_timing_flag;
    uint32_t         vui_num_ticks_poc_diff_one_minus1;
    uint8_t          vui_hrd_parameters_present_flag;
    HrdParameters    hrd_parameters;
    uint8_t          bitstream_restriction_flag;
    uint8_t          tiles_fixed_structure_flag;
    uint8_t          motion_vectors_over_pic_boundaries_flag;
    uint8_t          restricted_ref_pic_lists_flag;
    uint32_t         min_spatial_segmentation_idc;
    uint32_t         max_bytes_per_pic_denom;
    uint32_t         max_bits_per_min_cu_denom;
    uint32_t         log2_max_mv_length_horizontal;
    uint32_t         log2_max_mv_length_vertical;

    void toDefault();

    //bool operator == (const VuiParameters &) const;    

  };



  class NALUnit
  {
    public:
      NALUnit(NALUnitType type);
      virtual ~NALUnit();
      virtual NALUnitType getType() const;
      
      std::shared_ptr<NALUnit> copy() const;

      bool            m_processFailed;

      NALUnitType     m_nalUnitType;
  };



  class VPS: public NALUnit
  {
    public:
      VPS();
      uint8_t                   vps_video_parameter_set_id;
      uint8_t                   vps_max_layers_minus1;
      uint8_t                   vps_max_sub_layers_minus1;
      uint8_t                   vps_temporal_id_nesting_flag;
      ProfileTierLevel          profile_tier_level;
      uint8_t                   vps_sub_layer_ordering_info_present_flag;
      std::vector<uint32_t>     vps_max_dec_pic_buffering_minus1;
      std::vector<uint32_t>     vps_max_num_reorder_pics;
      std::vector<uint32_t>     vps_max_latency_increase_plus1;
      uint8_t                   vps_max_layer_id;
      uint32_t                  vps_num_layer_sets_minus1;
      std::vector<std::vector<uint8_t> > 
                                layer_id_included_flag;
      uint8_t                   vps_timing_info_present_flag;
      uint32_t                  vps_num_units_in_tick;
      uint32_t                  vps_time_scale;
      uint8_t                   vps_poc_proportional_to_timing_flag;
      uint32_t                  vps_num_ticks_poc_diff_one_minus1;
      uint32_t                  vps_num_hrd_parameters;
      std::vector<uint32_t>     hrd_layer_set_idx;
      std::vector<uint8_t>      cprms_present_flag;
      std::vector<HrdParameters>
                                hrd_parameters;
      uint8_t                   vps_extension_flag;

      void toDefault();
      //bool operator == (const VPS &) const;
  };

  
  class SPS: public NALUnit
  {
    public:
      SPS();
      uint8_t                  sps_video_parameter_set_id;
      uint8_t                  sps_max_sub_layers_minus1;
      uint8_t                  sps_temporal_id_nesting_flag;
      ProfileTierLevel         profile_tier_level;
      uint32_t                 sps_seq_parameter_set_id;
      uint32_t                 chroma_format_idc;
      uint8_t                  separate_colour_plane_flag;
      uint32_t                 pic_width_in_luma_samples;
      uint32_t                 pic_height_in_luma_samples;
      uint8_t                  conformance_window_flag;
      uint32_t                 conf_win_left_offset;
      uint32_t                 conf_win_right_offset;
      uint32_t                 conf_win_top_offset;
      uint32_t                 conf_win_bottom_offset;
      uint32_t                 bit_depth_luma_minus8;
      uint32_t                 bit_depth_chroma_minus8;
      uint32_t                 log2_max_pic_order_cnt_lsb_minus4;
      uint8_t                  sps_sub_layer_ordering_info_present_flag;
      std::vector<uint32_t>    sps_max_dec_pic_buffering_minus1;
      std::vector<uint32_t>    sps_max_num_reorder_pics;
      std::vector<uint32_t>    sps_max_latency_increase_plus1;
      uint32_t                 log2_min_luma_coding_block_size_minus3;
      uint32_t                 log2_diff_max_min_luma_coding_block_size;
      uint32_t                 log2_min_transform_block_size_minus2;
      uint32_t                 log2_diff_max_min_transform_block_size;
      uint32_t                 max_transform_hierarchy_depth_inter;
      uint32_t                 max_transform_hierarchy_depth_intra;
      uint8_t                  scaling_list_enabled_flag;
      uint8_t                  sps_scaling_list_data_present_flag;
      ScalingListData          scaling_list_data;
      uint8_t                  amp_enabled_flag;
      uint8_t                  sample_adaptive_offset_enabled_flag;
      uint8_t                  pcm_enabled_flag;
      uint8_t                  pcm_sample_bit_depth_luma_minus1;
      uint8_t                  pcm_sample_bit_depth_chroma_minus1;
      uint32_t                 log2_min_pcm_luma_coding_block_size_minus3;
      uint32_t                 log2_diff_max_min_pcm_luma_coding_block_size;
      uint8_t                  pcm_loop_filter_disabled_flag;
      uint32_t                 num_short_term_ref_pic_sets;
      std::vector<ShortTermRefPicSet>
                               short_term_ref_pic_set;
      uint8_t                  long_term_ref_pics_present_flag;
      uint32_t                 num_long_term_ref_pics_sps;
      std::vector<uint32_t>    lt_ref_pic_poc_lsb_sps;
      std::vector<uint8_t>     used_by_curr_pic_lt_sps_flag;
      uint8_t                  sps_temporal_mvp_enabled_flag;
      uint8_t                  strong_intra_smoothing_enabled_flag;
      uint8_t                  vui_parameters_present_flag;
      VuiParameters            vui_parameters;
      uint8_t                  sps_extension_flag;

      void toDefault();

      //bool operator == (const SPS &) const;
  };

  
  class PPS: public NALUnit
  {
    public:
    PPS();

    uint32_t     pps_pic_parameter_set_id;
    uint32_t     pps_seq_parameter_set_id;
    uint8_t      dependent_slice_segments_enabled_flag;
    uint8_t      output_flag_present_flag;
    uint8_t      num_extra_slice_header_bits;
    uint8_t      sign_data_hiding_flag;
    uint8_t      cabac_init_present_flag;
    uint32_t     num_ref_idx_l0_default_active_minus1;
    uint32_t     num_ref_idx_l1_default_active_minus1;
    int32_t      init_qp_minus26;   
    uint8_t      constrained_intra_pred_flag;
    uint8_t      transform_skip_enabled_flag;
    uint8_t      cu_qp_delta_enabled_flag;
    uint32_t     diff_cu_qp_delta_depth;
    int32_t      pps_cb_qp_offset;
    int32_t      pps_cr_qp_offset;
    uint8_t      pps_slice_chroma_qp_offsets_present_flag;
    uint8_t      weighted_pred_flag;
    uint8_t      weighted_bipred_flag;
    uint8_t      transquant_bypass_enabled_flag;
    uint8_t      tiles_enabled_flag;
    uint8_t      entropy_coding_sync_enabled_flag;
    uint32_t     num_tile_columns_minus1;
    uint32_t     num_tile_rows_minus1;
    uint8_t      uniform_spacing_flag;
    std::vector<uint32_t>  
                 column_width_minus1;
    std::vector<uint32_t>  
                 row_height_minus1;
    uint8_t      loop_filter_across_tiles_enabled_flag;
    uint8_t      pps_loop_filter_across_slices_enabled_flag;
    uint8_t      deblocking_filter_control_present_flag;
    uint8_t      deblocking_filter_override_enabled_flag;
    uint8_t      pps_deblocking_filter_disabled_flag;
    uint32_t     pps_beta_offset_div2;
    uint32_t     pps_tc_offset_div2;
    uint8_t      pps_scaling_list_data_present_flag;
    ScalingListData
                 scaling_list_data;
    uint8_t      lists_modification_present_flag;
    int32_t      log2_parallel_merge_level_minus2;
    uint8_t      slice_segment_header_extension_present_flag;
    uint8_t      pps_extension_flag;

    void toDefault();

    //bool operator == (const PPS &) const;
  };




  class AUD: public NALUnit
  {
  public:
    AUD();

    uint8_t            pic_type;
    void toDefault();
  };


}

#endif
